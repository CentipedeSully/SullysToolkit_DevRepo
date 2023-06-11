using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SullysToolkit
{
    public class TriggerConflict : MonoBehaviour, IInteractablePiece
    {
        //Declarations
        [SerializeField] private List<string> _enemyFactions;
        [SerializeField] private GamePiece _gamePieceRef;
        [SerializeField] private bool _isDebugActive = true;
        private IIdentityDefinition _identityRef;

        //Events
        public delegate void ConflictTriggeredEvent(GamePiece attacker, GamePiece defender);
        public event ConflictTriggeredEvent OnConflictTriggered;

        //Monobehaviours
        private void Awake()
        {
            InitializeReferences();
        }



        //Internal utils
        private void InitializeReferences()
        {
            _gamePieceRef = GetComponent<GamePiece>();
            _identityRef = _gamePieceRef.GetComponent<IIdentityDefinition>();
        }


        //Getters, Setters, & Commands
        public GamePiece GetGamePiece()
        {
            return _gamePieceRef;
        }

        public void TriggerInteractionEvent(GamePiece performer)
        {
            STKDebugLogger.LogStatement(_isDebugActive, $"Comparing the Identities of self(defender) and Attacker({performer})...");
            IIdentityDefinition identifiedGamePiece = performer.GetComponent<IIdentityDefinition>();

            if (_enemyFactions.Contains(identifiedGamePiece.GetFaction()))
            {
                STKDebugLogger.LogStatement(_isDebugActive, $"Enemy Verified! Entering COnflict!");
                ConflictResolver.ResolveTwoSidedConflict(performer, _gamePieceRef);
                OnConflictTriggered?.Invoke(performer, _gamePieceRef);
            }
            else
                STKDebugLogger.LogStatement(_isDebugActive,$"The interaction Performer isn't an enemy of this gamePiece. Ignoring Conflict.");
                
        }
    }
}

