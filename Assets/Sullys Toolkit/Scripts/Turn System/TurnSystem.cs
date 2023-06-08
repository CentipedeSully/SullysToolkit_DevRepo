using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace SullysToolkit
{
    public enum TurnPhase
    {
        RefreshPhase,
        MainPhase,
        ReactionPhase,
        TurnOverPhase
    }

    public interface ITurnListener
    {
        int GetResponsePhase();

        bool IsTurnListenerReadyToPassPhase();

        void RespondToNotification(int turnNumber);

        void ResetResponseFlag();

        ITurnBroadcaster GetTurnBroadcaster();

        string GetConcreteListenerNameForDebugging();

        void ResetUtilsOnTurnSystemInterruption();


    }

    public interface ITurnBroadcaster
    {
        int GetCurrentTurn();

        int GetCurrentPhase();

        int GetMaxTurnLimit();

        void NotifyTurnListenersOfPhaseChange();

        void AddTurnListener(ITurnListener listener);

        void RemoveTurnListener(ITurnListener listener);

        void StartTurnSystem();

        void StopTurnSystem();

        bool IsTurnSystemActive();
    }

    public class TurnSystem : MonoBehaviour, ITurnBroadcaster 
    {
        //Declarations
        [Header("Turn System Settings")]
        [SerializeField] private bool _isTurnSystemActive = false;
        [SerializeField] private int _currentTurnCount = 0;
        [SerializeField] private int _maxTurnCount = 0;
        [SerializeField] private TurnPhase _currentPhase = 0;
        private List<List<ITurnListener>> _listenersList;
        private IEnumerator _turnManager;

        [Header("Debugging Utilities")]
        [SerializeField] List<int> _listenersPerPhaseList;
        [SerializeField] private bool _isDebugActive;


        //Events
        public delegate void TurnSystemEvent();
        public event TurnSystemEvent OnTurnSystemInterrupted;
        public event TurnSystemEvent OnMaxTurnCountReached;


        //Monobehaviours
        private void Awake()
        {
            InitializeListenerLists();
        }

        private void OnDestroy()
        {
            FreeSubscriptionsFromMemory();
        }



        //Internal utils
        private void SubscribeListenerToInterruptionEvent(ITurnListener validListener)
        {
            OnTurnSystemInterrupted += validListener.ResetUtilsOnTurnSystemInterruption;
        }

        private void UnsubscribeListenerFromInterruptionEvent(ITurnListener validListener)
        {
            OnTurnSystemInterrupted -= validListener.ResetUtilsOnTurnSystemInterruption;
        }

        private void FreeSubscriptionsFromMemory()
        {
            if (_listenersList != null)
            {
                foreach (List<ITurnListener> phaseList in _listenersList)
                {
                    foreach (ITurnListener listener in phaseList)
                        UnsubscribeListenerFromInterruptionEvent(listener);
                }
            }
        }


        //Interface Utils
        public int GetCurrentPhase()
        {
            return (int)_currentPhase;
        }

        public int GetCurrentTurn()
        {
            return _currentTurnCount;
        }

        public int GetMaxTurnLimit()
        {
            return _maxTurnCount;
        }

        public int GetListenerCount(int phase)
        {
            if (phase < System.Enum.GetNames(typeof(TurnPhase)).Length)
                return _listenersList[phase].Count;
            else return 0;
        }

        public List<ITurnListener> GetListenersInPhase(int phase)
        {
            if (phase < System.Enum.GetNames(typeof(TurnPhase)).Length)
                return _listenersList[phase];
            else return new List<ITurnListener>();
        }

        public void NotifyTurnListenersOfPhaseChange()
        {
            //Make all listeners of the curent phase respond to this current phase
            foreach (ITurnListener listener in _listenersList[(int)_currentPhase])
                listener.RespondToNotification(_currentTurnCount);
        }

        public void AddTurnListener(ITurnListener listener)
        {
            //check if the response phase of the listener is a valid phase for the turn system
            if (listener.GetResponsePhase() < _listenersList.Count)
            {
                //Check if the listener already exists
                foreach (ITurnListener preExistingListener in _listenersList[listener.GetResponsePhase()])
                {
                    //if the names match, then the listener already exists. State a warning and return.
                    if (listener.GetConcreteListenerNameForDebugging() == preExistingListener.GetConcreteListenerNameForDebugging())
                    {
                        Debug.LogWarning($"Attempting to add preExisting Listener ({listener.GetConcreteListenerNameForDebugging()}) " +
                                         $"to the Turn System. Ignoring Command.");
                        return;
                    }
                }

                //Don't add the listener if both the following are true:
                //1) turn systems is active
                //2) the listener's phase matches the currently waiting phase of the turn
                //reason why: C# lists can't be modified while being iterated through
                if (_isTurnSystemActive && listener.GetResponsePhase() == (int)_currentPhase)
                {
                    STKDebugLogger.LogWarning("Attempted to add a new turnListener to the current turn phase.\n" +
                        "Try adding it during another phase");
                    return;
                }
                    

                //Add listener to the appropriate list, if it doesn't exist already
                _listenersList[listener.GetResponsePhase()].Add(listener);
                STKDebugLogger.LogStatement(_isDebugActive,$"Added newListener '{listener.GetConcreteListenerNameForDebugging()}' " +
                    $"to the '{(TurnPhase)listener.GetResponsePhase()}' phase of the Turn System.");

                //Increment count
                _listenersPerPhaseList[listener.GetResponsePhase()] += 1;

                //Subscribe Listener to the OnInterrupt event
                SubscribeListenerToInterruptionEvent(listener);
                    
            }
                
        }

        public void RemoveTurnListener(ITurnListener listener)
        {
            //if the response phase of the listener is a valid phase for the turn system, then remove it to the respective phase's list
            if (listener.GetResponsePhase() < _listenersList.Count)
            {
                bool listenerFound = _listenersList[listener.GetResponsePhase()].Remove(listener);
                STKDebugLogger.LogStatement(_isDebugActive, $"Removed listener '{listener.GetConcreteListenerNameForDebugging()}' " +
                        $"of the '{(TurnPhase)listener.GetResponsePhase()}' phase from the Turn System.");

                if (listenerFound)
                {
                    UnsubscribeListenerFromInterruptionEvent(listener);

                    //Decrement the count
                    _listenersPerPhaseList[listener.GetResponsePhase()] -= 1;
                }
                    
            }
        }

        public void StartTurnSystem()
        {
            if (_isTurnSystemActive == false)
            {
                _isTurnSystemActive = true;
                STKDebugLogger.LogStatement(_isDebugActive, "Starting TurnSystem...");
                _turnManager = ManageTurnLifecycles();
                StartCoroutine(_turnManager);
            }
        }

        public void StopTurnSystem()
        {
            if (_isTurnSystemActive)
            {
                _isTurnSystemActive = false;
                StopCoroutine(_turnManager);
                _turnManager = null;
                OnTurnSystemInterrupted?.Invoke();
                _currentPhase = 0;
                _currentTurnCount = 0;
            }
        }

        public bool IsTurnSystemActive()
        {
            return _isTurnSystemActive;
        }


        //Utils
        private void InitializeListenerLists()
        {
            _listenersList = new List<List<ITurnListener>>();
            _listenersPerPhaseList = new List<int>();

            //Create a list for each phase
            for (int i = 0; i < System.Enum.GetNames(typeof(TurnPhase)).Length; i++)
            {
                _listenersList.Add(new List<ITurnListener>());
                _listenersPerPhaseList.Add(0);
            }
                
        }

        private IEnumerator ManageTurnLifecycles()
        {
            //decide whether or not this turn system is endless
            bool isMaxTurnLimitInfinite = false;
            if (_maxTurnCount < 1)
                isMaxTurnLimitInfinite = true;


            //init the phase metadata
            int phaseCounter;
            int numberOfPhases = System.Enum.GetNames(typeof(TurnPhase)).Length;


            //begin managing turns. Either cycle through the phases until the max turn is reached, or cycle endlessly
            while (_currentTurnCount < _maxTurnCount || isMaxTurnLimitInfinite)
            {
                //Reset the turn back to the beginning phase
                phaseCounter = 0;

                while (phaseCounter < numberOfPhases)
                {
                    //Update the turnSystem's phase and then notify all relevant IListeners
                    _currentPhase = (TurnPhase)phaseCounter;
                    STKDebugLogger.LogStatement(_isDebugActive, $"New Phase started: {_currentPhase}");
                    NotifyTurnListenersOfPhaseChange();


                    //Wait until all IListeners finish their respective response...
                    bool areAllListenersReady = false;
                    while (areAllListenersReady == false)
                    {
                        //assume all are ready until one is verified to not be ready
                        areAllListenersReady = true;

                        //foreach listener of the current turn phase, (Do above comment ^)
                        foreach (ITurnListener listener in _listenersList[(int)_currentPhase])
                        {
                            if (listener.IsTurnListenerReadyToPassPhase() == false)
                            {
                                areAllListenersReady = false;
                                break;
                            }

                        }

                        //wait one frame before checking again
                        yield return null;
                    }

                    //All IListeners have completed their respective response.
                    //Reset their response flags and then Increment the phaseCounter

                    foreach (ITurnListener listener in _listenersList[(int)_currentPhase])
                        listener.ResetResponseFlag();

                    STKDebugLogger.LogStatement(_isDebugActive, $"{(TurnPhase)phaseCounter} Ended.");
                    phaseCounter++;
                }
                STKDebugLogger.LogStatement(_isDebugActive, $"End of turn {_currentTurnCount} reached.");
                _currentTurnCount++;
            }

            //Communicate turns over
            OnMaxTurnCountReached?.Invoke();
        }

    }

}

