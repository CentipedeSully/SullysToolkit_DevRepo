using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace SullysToolkit
{
    public class ExpPool : MonoBehaviour, IInteractablePiece, IExperienceProvider
    {
        //Declarations
        [Header("Settings")]
        [SerializeField] [Min(0)] private int _expValue = 5;
        [SerializeField] private bool _isPoolAvailable = false;
        [SerializeField] private GamePiece _gamePieceRef;
        [SerializeField] private bool _isDebugActive = false;
        

        //Events
        public delegate void InteractionEvent(GamePiece eventSource, GamePiece whatTriggeredThisEvent);
        public event InteractionEvent OnEventTriggered;

        //Monobehaviours
        private void Awake()
        {
            InitializeReferences();
        }


        //Internal Utils
        private void InitializeReferences()
        {
            _gamePieceRef = GetComponent<GamePiece>();
        }

        private void GrantExpToPerformerAndTriggerEvent(ILevelablePiece gamePiece)
        {
            if (gamePiece != null)
            {
                STKDebugLogger.LogStatement(_isDebugActive,$"Granting {_expValue}(XP) to {gamePiece}");
                _isPoolAvailable = false;
                gamePiece.GainExp(_expValue);
                OnEventTriggered?.Invoke(_gamePieceRef, gamePiece.GetGamePiece());
            }
            else
                STKDebugLogger.LogStatement(_isDebugActive, $"{gamePiece} has no ILevelable interface. Ignoring ExpGrant Command");

        }


        //Getters, Setters, & Commands
        public GamePiece GetGamePiece()
        {
            return _gamePieceRef;
        }

        public void TriggerInteractionEvent(GamePiece performer)
        {
            ILevelablePiece levelableGamePiece = performer.GetComponent<ILevelablePiece>();
            GrantExpToPerformerAndTriggerEvent(levelableGamePiece);
        }

        public int GetExpValue()
        {
            return _expValue;
        }

        public void SetExpValue(int value)
        {
            _expValue = Mathf.Max(0, value);
        }

        public void ResetPool()
        {
            _isPoolAvailable = true;
        }

        public bool IsPoolAvailable()
        {
            return _isPoolAvailable;
        }
    }
}


