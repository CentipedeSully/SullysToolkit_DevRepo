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
        private IAttributes _attributesRef;

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
            _attributesRef = _gamePieceRef.GetComponent<IAttributes>();
        }


        //Getters, Setters, & Commands
        public GamePiece GetGamePiece()
        {
            return _gamePieceRef;
        }

        public void TriggerInteractionEvent(GamePiece performer)
        {
            IAttributes attackerAttributes = performer?.GetComponent<IAttributes>();
            if (attackerAttributes == null)
            {
                STKDebugLogger.LogError($"gamePiece {performer} has no attributes to deduct Ap from");
                return;
            }

            else if (attackerAttributes.GetCurrentActionPoints()  > 0)
            {
                STKDebugLogger.LogStatement(_isDebugActive, $"Comparing the Identities of self(defender) and Attacker({performer})...");
                IIdentityDefinition identifiedGamePiece = performer.GetComponent<IIdentityDefinition>();

                if (_enemyFactions.Contains(identifiedGamePiece.GetFaction()))
                {
                    STKDebugLogger.LogStatement(_isDebugActive, $"Interaction with Hostile verified! Determining Conflict type");
                    if (_attributesRef.GetCurrentActionPoints() < 1)
                    {
                        STKDebugLogger.LogStatement(_isDebugActive, $"Entering OneSided Conflict due to defender having insufficient AP. Deducting Ap from attacker");
                        ConflictResolver.ResolveOneSidedConflict(performer, _gamePieceRef);
                    }
                    else
                    {
                        STKDebugLogger.LogStatement(_isDebugActive, $"Entering Two-Sided Conflict and Deducting AP fom both parties.");
                        ConflictResolver.ResolveTwoSidedConflict(performer, _gamePieceRef);
                    }

                    OnConflictTriggered?.Invoke(performer, _gamePieceRef);
                }
                else
                    STKDebugLogger.LogStatement(_isDebugActive, $"The interaction Performer isn't an enemy of this gamePiece. Ignoring Conflict.");

            }

            else
                STKDebugLogger.LogStatement(_isDebugActive,$"Interaction Failed. {performer} has no AP");

        }
    }
}

