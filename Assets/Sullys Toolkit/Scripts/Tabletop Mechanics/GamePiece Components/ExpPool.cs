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
            if (gamePiece == null)
            {
                STKDebugLogger.LogStatement(_isDebugActive,$"Performer {gamePiece} of ExpPool Interaction is null. Ignoring interaction");
                return;
            }


            IAttributes gPieceAtrtibutes = gamePiece.GetGamePiece().GetComponent<IAttributes>();
            if (gPieceAtrtibutes == null)
            {
                STKDebugLogger.LogStatement(_isDebugActive, $"Performer {gamePiece} of ExpPool Interaction has No Attributes. Ignoring interaction");
                return;
            }


            if (gPieceAtrtibutes.GetCurrentActionPoints() > 1)
            {
                //Deduct AP
                gPieceAtrtibutes.SetCurrentActionPoints(gPieceAtrtibutes.GetCurrentActionPoints() - 1);

                STKDebugLogger.LogStatement(_isDebugActive,$"Granting {_expValue}(XP) to {gamePiece} and Deducting Ap");
                _isPoolAvailable = false;
                gamePiece.GainExp(_expValue);
                OnEventTriggered?.Invoke(_gamePieceRef, gamePiece.GetGamePiece());
            }
            else
                STKDebugLogger.LogStatement(_isDebugActive, $"{gamePiece} has no Ap for this interaction. Ignoring Interaction");

        }


        //Getters, Setters, & Commands
        public GamePiece GetGamePiece()
        {
            return _gamePieceRef;
        }

        public void TriggerInteractionEvent(GamePiece performer)
        {
            ILevelablePiece levelableGamePiece = performer?.GetComponent<ILevelablePiece>();
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


