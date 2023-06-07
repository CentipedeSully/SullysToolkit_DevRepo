using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SullysToolkit
{
    public class TurnSystemObjectRegenerator : MonoBehaviour, ITurnListener
    {
        //Declarations
        [Header("Regeneration Timing Utils")]
        [SerializeField] private TurnSystem _turnSystem;
        [SerializeField] private TurnPhase _regenPhase;
        [SerializeField] private IRegenerateable[] _regenerateableReferences;
        [SerializeField] private bool _readyToPassTurn = false;

        //Monos
        private void Awake()
        {
            InitializeReferences();
        }



        //Internals
        private void InitializeReferences()
        {
            _regenerateableReferences = GetComponents<IRegenerateable>();
            _turnSystem.AddTurnListener(this);
        }



        // Getters, Setters, && Commands
        public void TriggerRegenerationInChildReferences()
        {
            foreach (IRegenerateable reference in _regenerateableReferences)
            {
                if (reference != null)
                    reference.RegenerateAttributes();
            }
                
        }

        public int GetResponsePhase()
        {
            return (int)_regenPhase;
        }

        public bool IsTurnListenerReadyToPassPhase()
        {
            return _readyToPassTurn;
        }

        public void RespondToNotification(int turnNumber)
        {
            TriggerRegenerationInChildReferences();
            _readyToPassTurn = true;
        }

        public void ResetResponseFlag()
        {
            _readyToPassTurn = false;
        }

        public ITurnBroadcaster GetTurnBroadcaster()
        {
            return _turnSystem;
        }

        public string GetConcreteListenerNameForDebugging()
        {
            return gameObject.name;
        }
    }
}

