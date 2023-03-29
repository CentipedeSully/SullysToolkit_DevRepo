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
    }

    public class TurnSystem : MonoBehaviour, ITurnBroadcaster 
    {
        //Declarations
        [SerializeField] private int _currentTurnCount = 0;

        [SerializeField] private int _maxTurnCount = 0;

        [SerializeField] private TurnPhase _currentPhase = 0;

        private List<List<ITurnListener>> _listenersList;


        //Events
        public delegate void TurnSystemEvent();
        public event TurnSystemEvent OnMaxTurnCountReached;


        //Monobehaviours
        private void Awake()
        {
            InitializeListenerLists();
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
                bool isListenerAlreadyInCollection = false;
                foreach (ITurnListener preExistingListener in _listenersList[listener.GetResponsePhase()])
                {
                    //if the names match, then the listener already exists. Raise the flag and break.
                    if (listener.GetConcreteListenerNameForDebugging() == preExistingListener.GetConcreteListenerNameForDebugging())
                    {
                        isListenerAlreadyInCollection = true;
                        Debug.LogWarning($"Attempting to add preExisting Listener ({listener.GetConcreteListenerNameForDebugging()}) " +
                                         $"to the Turn System. Ignoring Command.");
                        break;
                    }
                }

                //Add listener to the appropriate list, if it doesn't exist already
                if (isListenerAlreadyInCollection == false)
                    _listenersList[listener.GetResponsePhase()].Add(listener);
            }
                
        }

        public void RemoveTurnListener(ITurnListener listener)
        {
            //if the response phase of the listener is a valid phase for the turn system, then remove it to the respective phase's list
            if (listener.GetResponsePhase() < _listenersList.Count)
                _listenersList[listener.GetResponsePhase()].Remove(listener);
        }

        public void StartTurnSystem()
        {
            StartCoroutine(ManageTurnLifecycles());
        }


        //Utils
        private void InitializeListenerLists()
        {
            _listenersList = new List<List<ITurnListener>>();

            //Create a list for each phase
            for (int i = 0; i < System.Enum.GetNames(typeof(TurnPhase)).Length; i++)
                _listenersList.Add(new List<ITurnListener>());
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
                    phaseCounter++;
                }

                _currentTurnCount++;
            }

            //Communicate turns over
            OnMaxTurnCountReached?.Invoke();
        }

    }


    public class TestTurnListener : ITurnListener
    {
        //Declarations
        int _responsePhase;
        bool _isListenerTaskStarted = false;
        bool _isListenerTaskCompleted = false;
        string _ListenerName = "Unnamed Listener";
        ITurnBroadcaster _broadcaster;
        SimpleTimer _taskTimer;


        //Constructors
        public TestTurnListener(int turnPhase, string name, SimpleTimer taskTimer, ITurnBroadcaster turnSystem)
        {
            if (turnPhase >= 0)
                this._responsePhase = turnPhase;

            this._ListenerName = name;

            this._taskTimer = taskTimer;

            //Make sure the timer is set to something
            if (taskTimer.GetLifespan() == 0)
                taskTimer.SetLifespan(1);

            _broadcaster = turnSystem;
        }


        //Interface Utils
        public int GetResponsePhase()
        {
            return _responsePhase;
        }

        public string GetConcreteListenerNameForDebugging()
        {
            return _ListenerName;
        }

        public bool IsTurnListenerReadyToPassPhase()
        {
            if (_isListenerTaskStarted && _isListenerTaskCompleted)
                return true;
            else return false;
        }

        public void RespondToNotification(int turnNumber)
        {
            StartTask();
        }

        public ITurnBroadcaster GetTurnBroadcaster()
        {
            return _broadcaster;
        }

        public void ResetResponseFlag()
        {
            //unsubscribe
            _taskTimer.OnTimerExpired -= FlagTaskAsComplete;

            _isListenerTaskCompleted = false;
            _isListenerTaskStarted = false;

            //log status
            Debug.Log($"{_ListenerName}, (phase{_responsePhase}) flags reset");
        }


        //Utils
        private void StartTask()
        {
            _isListenerTaskStarted = true;

            //subscribe
            _taskTimer.OnTimerExpired += FlagTaskAsComplete;

            //Log status
            Debug.Log($"{_ListenerName}, (phase{_responsePhase}) task started. Beginning Timer");
            _taskTimer.StartTimer();

        }

        private void FlagTaskAsComplete()
        {
            _isListenerTaskCompleted = true;
            Debug.Log($"{_ListenerName}, (phase{_responsePhase}) completed.");
        }


    }

}

