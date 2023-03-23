using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
        private int _currentTurnCount = 1;

        private int _maxTurnCount = 1;

        private TurnPhase _currentPhase = 0;

        private List<List<ITurnListener>> _listenersList;



        //Constructor
        public TurnSystem(int maxTurnCount = 0)
        {
            if (maxTurnCount > 1)
                this._maxTurnCount = maxTurnCount;

            //Create a list for each phase
            for (int i = 0; i < System.Enum.GetNames(typeof(TurnPhase)).Length; i++)
                _listenersList.Add(new List<ITurnListener>());
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

        public void NotifyTurnListenersOfPhaseChange()
        {
            //Make all listeners of the curent phase respond to this current phase
            foreach (ITurnListener listener in _listenersList[(int)_currentPhase])
                listener.RespondToNotification(_currentTurnCount);
        }

        public void AddTurnListener(ITurnListener listener)
        {
            //if the response phase of the listener is a valid phase for the turn system, then add it to the respective phase's list
            if (listener.GetResponsePhase() < _listenersList.Count)
                _listenersList[listener.GetResponsePhase()].Add(listener);
        }

        public void RemoveTurnListener(ITurnListener listener)
        {
            //if the response phase of the listener is a valid phase for the turn system, then remove it to the respective phase's list
            if (listener.GetResponsePhase() < _listenersList.Count)
                _listenersList[listener.GetResponsePhase()].Remove(listener);
        }

        public void StartTurnSystem()
        {
            StartCoroutine(ManageTurnPhases());
        }

        //Utils
        private IEnumerator ManageTurnPhases()
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

                    //All IListeners have completed their respective response. Increment the phaseCounter
                    phaseCounter++;
                }

                _currentTurnCount++;
            }

            //Communicate turns over
        }

    }
}

