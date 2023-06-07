using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SullysToolkit
{
    public class GameBoardAndGamePieceTester : MonoBehaviour, IConflictLogger
    {
        //Delcarations
        [Header("Command Settings")]
        [SerializeField] private int _xPosition;
        [SerializeField] private int _yPosition;
        [SerializeField] private int _xMoveDirection;
        [SerializeField] private int _yMoveDirection;
        [SerializeField] private GamePiece _targetSelection;
        [SerializeField] private List<GamePiece> _countedPiecesOnPosition;


        [Header("Add/Remove Testing Commands")]
        [SerializeField] private bool _addNewUnitToBoard;
        [SerializeField] private bool _addNewTerrainToBoard;
        [SerializeField] private bool _addNewPOIToBoard;
        [SerializeField] private bool _removeSelectionFromGameboard;
        [SerializeField] private bool _addSelectionToGameboard;
        [SerializeField] private bool _countPiecesOnPosition;

        [Header("Movement Testing Commands")]
        [SerializeField] private bool _moveSelectionInDirection;

        [Header("Health Management Commands")]
        [SerializeField] private int _healthModifer;
        [SerializeField] private bool _setCurrentHealth;
        [SerializeField] private bool _setMaxHealth;
        [SerializeField] private bool _damagePiece;
        [SerializeField] private bool _healPiece;

        [Header("Growth Testing Commands")]
        [SerializeField] private int _expModifier;
        [SerializeField] private int _desiredLv;
        [SerializeField] private bool _lvUpSelection;
        [SerializeField] private bool _SetLvOnSelection;
        [SerializeField] private bool _addExpToSelection;

        [Header("Conflict Testint Commands")]
        [SerializeField] private GamePiece _attacker;
        [SerializeField] private GamePiece _defender;
        [SerializeField] private int _loggedAttackerAtkRoll;
        [SerializeField] private int _loggedAttackerDmgRoll;
        [SerializeField] private int _loggedAttackerDef;
        [SerializeField] private int _loggedDefenderAtkRoll;
        [SerializeField] private int _loggedDefenderDmgRoll;
        [SerializeField] private int _loggedDefenderDef;

        [SerializeField] private bool _enterOneSidedConflict;
        [SerializeField] private bool _enterTwoSidedConflict;



        [Header("References")]
        [SerializeField] private GameBoard _gameBoardReference;
        [SerializeField] private Transform _bagOfHoldingReference;

        [SerializeField] private GamePiece _unitPiecePrefab;
        [SerializeField] private GamePiece _terrainPiecePrefab;
        [SerializeField] private GamePiece _POIPiecePrefab;



        //Monobehaviours
        private void Awake()
        {
            SetupConflictLogging();
        }

        private void Update()
        {
            ListenForDebugCommands();
        }



        //Internal Utils
        private void ListenForDebugCommands()
        {
            //Adding/Removing
            if (_addNewUnitToBoard)
            {
                _addNewUnitToBoard = false;
                AddGamePieceToGameBoard(CreateNewGamePiece(_unitPiecePrefab));
            }

            if (_addNewTerrainToBoard)
            {
                _addNewTerrainToBoard = false;
                AddGamePieceToGameBoard(CreateNewGamePiece(_terrainPiecePrefab));
            }

            if (_addNewPOIToBoard)
            {
                _addNewPOIToBoard = false;
                AddGamePieceToGameBoard(CreateNewGamePiece(_POIPiecePrefab));
            }

            if (_addSelectionToGameboard)
            {
                _addSelectionToGameboard = false;
                if (_targetSelection != null)
                    AddGamePieceToGameBoard(_targetSelection);
            }

            if (_removeSelectionFromGameboard)
            {
                _removeSelectionFromGameboard = false;
                if (_targetSelection != null)
                    RemoveGamePieceFromGameBoard(_targetSelection);
            }

            if (_countPiecesOnPosition)
            {
                _countPiecesOnPosition = false;
                CountGamePiecesOnSpecifiedPosition();
            }

            //Movement
            if (_moveSelectionInDirection)
            {
                _moveSelectionInDirection = false;
                if (_targetSelection != null)
                    MovePiece(_targetSelection);
            }

            //Health Management
            if (_setCurrentHealth)
            {
                _setCurrentHealth = false;
                if (_targetSelection != null)
                    SetCurrentGamePieceHealth(_targetSelection);
            }

            if (_setMaxHealth)
            {
                _setMaxHealth = false;
                if (_targetSelection != null)
                    SetGamePieceMaxHealth(_targetSelection);

            }

            if (_damagePiece)
            {
                _damagePiece = false;
                if (_targetSelection != null)
                    DamagePiece(_targetSelection);
            }

            if (_healPiece)
            {
                _healPiece = false;
                if (_targetSelection != null)
                    HealPiece(_targetSelection);
            }

            //Growth/Lvling Up
            if (_addExpToSelection)
            {
                _addExpToSelection = false;
                if (_targetSelection != null)
                    AddExpToPiece(_targetSelection);
            }

            if (_lvUpSelection)
            {
                _lvUpSelection = false;
                if (_targetSelection != null)
                    LvUpPiece(_targetSelection);
            }

            if (_SetLvOnSelection)
            {
                _SetLvOnSelection = false;
                if (_targetSelection != null)
                    SetPieceLvToDesiredLv(_targetSelection);
            }
            
            //Conflict Management
            if (_enterOneSidedConflict)
            {
                _enterOneSidedConflict = false;
                EnterOneSidedConflict(_attacker, _defender);
            }

            if (_enterTwoSidedConflict)
            {
                _enterTwoSidedConflict = false;
                EnterTwoSidedConflict(_attacker, _defender);
            }

        }

        private void CountGamePiecesOnSpecifiedPosition()
        {
            _countedPiecesOnPosition = _gameBoardReference.GetPiecesOnPosition((_xPosition, _yPosition));
        }

        private void AddGamePieceToGameBoard(GamePiece gamePiece)
        {
            (int, int) xyPosition = (_xPosition, _yPosition);
            _gameBoardReference.AddGamePiece(gamePiece, gamePiece.GetBoardLayer(), xyPosition);
        }

        private void RemoveGamePieceFromGameBoard(GamePiece gamePiece)
        {
            _gameBoardReference.RemoveGamePieceFromBoard(gamePiece);
        }

        private GamePiece CreateNewGamePiece(GamePiece prefab)
        {
            GamePiece newPiece = Instantiate(prefab.gameObject, _bagOfHoldingReference).GetComponent<GamePiece>();
            newPiece.SetOutOfPlayHoldingLocation(_bagOfHoldingReference);
            return newPiece;
        }

        private void MovePiece(GamePiece gamePiece)
        {
            IMoveablePiece moveablePiece = gamePiece.GetComponent<IMoveablePiece>();

            if (moveablePiece != null)
                moveablePiece.MoveToNeighborCell((_xPosition, _yPosition));
        }

        private void SetGamePieceMaxHealth(GamePiece gamePiece)
        {
            IHealthManager pieceHealthRef = gamePiece.GetComponent<IHealthManager>();
            if (pieceHealthRef != null)
                pieceHealthRef.SetMaxHealth(_healthModifer);
        }

        private void SetCurrentGamePieceHealth(GamePiece gamePiece)
        {
            IHealthManager pieceHealthRef = gamePiece.GetComponent<IHealthManager>();
            if (pieceHealthRef != null)
                pieceHealthRef.SetCurrentHealth(_healthModifer);
        }

        private void DamagePiece(GamePiece gamepiece)
        {
            IDamageablePiece damageablePiece = gamepiece.GetComponent<IDamageablePiece>();
            if (damageablePiece != null)
                damageablePiece.RecieveDamage(_healthModifer);
        }

        private void HealPiece(GamePiece gamepiece)
        {
            IHealablePiece healablePiece = gamepiece.GetComponent<IHealablePiece>();
            if (healablePiece != null)
                healablePiece.ReceiveHeals(_healthModifer);
        }

        private void LvUpPiece(GamePiece gamePiece)
        {
            ILevelablePiece _lvlableGamePiece = gamePiece.GetComponent<ILevelablePiece>();

            if (_lvlableGamePiece != null)
                _lvlableGamePiece.LvUp();
        }

        private void SetPieceLvToDesiredLv(GamePiece gamePiece)
        {
            ILevelablePiece _lvlableGamePiece = gamePiece.GetComponent<ILevelablePiece>();

            if (_lvlableGamePiece != null)
                _lvlableGamePiece.SetCurrentLv(_desiredLv);
        }

        private void AddExpToPiece(GamePiece gamePiece)
        {
            ILevelablePiece _lvlableGamePiece = gamePiece.GetComponent<ILevelablePiece>();

            if (_lvlableGamePiece != null)
                _lvlableGamePiece.GainExp(_expModifier);
        }

        private void SetupConflictLogging()
        {
            ConflictResolver.SetConflictLogger(this);
        }

        private void EnterOneSidedConflict(GamePiece attacker, GamePiece defender)
        {
            if (attacker != null && defender != null)
                ConflictResolver.ResolveOneSidedConflict(attacker, defender);
        }

        private void EnterTwoSidedConflict(GamePiece attacker, GamePiece defender)
        {
            if (attacker != null && defender != null)
                ConflictResolver.ResolveTwoSidedConflict(attacker, defender);
        }


        //Getters, Setters, & Commands
        public void LogConflict(int attackerAtkScore, int attackerDmgScore, int attackerDef, int defenderAtkScore, int defenderDmgScore, int defenderDef)
        {
            _loggedAttackerAtkRoll = attackerAtkScore;
            _loggedAttackerDmgRoll = attackerDmgScore;
            _loggedAttackerDef = attackerDef;
            _loggedDefenderAtkRoll = defenderAtkScore;
            _loggedDefenderDmgRoll = defenderDmgScore;
            _loggedDefenderDef = defenderDef;
        }




    }
}

