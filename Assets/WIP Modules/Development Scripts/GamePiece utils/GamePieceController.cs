using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SullysToolkit
{
    public class GamePieceController : MonoBehaviour, IGamePieceController, ITurnListener
    {
        //Declarations
        [Header("Controller Settings")]
        [SerializeField] private TurnPhase _controllablePhase;
        [SerializeField] private TurnSystem _turnSystem;
        [SerializeField] private bool _isControlAvailable = false;
        [SerializeField] private bool _isUIDataShowing = false;

        private GamePiece _gamePieceReference;
        private IMoveablePiece _movementReference;
        private IIdentityDefinition _identityReference;




        [Header("Debugging Utils")]
        [SerializeField] private bool _isDebugActive = false;
        



        //Monobehaviours
        private void Awake()
        {
            InitializeReferences();
        }


        //Internal Utils
        private void InitializeReferences()
        {
            _gamePieceReference = GetComponent<GamePiece>();
            _identityReference = _gamePieceReference.GetComponent<IIdentityDefinition>();
            _movementReference = _gamePieceReference.GetComponent<IMoveablePiece>();
        }



        //Getters, Setters, & Commands
        public bool IsDebugActive()
        {
            return _isDebugActive;
        }

        public bool IsControlAvailable()
        {
            return _isControlAvailable;
        }

        public GamePiece GetGamePiece()
        {
            return _gamePieceReference;
        }

        public void InteractWithCellInDirection((int, int) xyDirection)
        {
            throw new System.NotImplementedException();
        }

        public bool IsShowDataToggled()
        {
            return _isUIDataShowing;
        }

        public void MoveInDirection((int, int) xyDirection)
        {
            if (_movementReference != null)
                _movementReference.MoveToNeighborCell(xyDirection);
        }

        public void ToggleShowData(bool value)
        {
            _isUIDataShowing = value;
        }

        public int GetResponsePhase()
        {
            return (int)_controllablePhase;
        }

        public bool IsTurnListenerReadyToPassPhase()
        {
            return true;
        }

        public void RespondToNotification(int turnNumber)
        {
            _isControlAvailable = true;
        }

        public void ResetResponseFlag()
        {
            _isControlAvailable = false;
        }

        public ITurnBroadcaster GetTurnBroadcaster()
        {
            return _turnSystem;
        }

        public string GetConcreteListenerNameForDebugging()
        {
            return ToString() + ", ID:" + GetInstanceID();
        }

        public void ResetUtilsOnTurnSystemInterruption()
        {
            _isControlAvailable = false;
        }
    }
}

