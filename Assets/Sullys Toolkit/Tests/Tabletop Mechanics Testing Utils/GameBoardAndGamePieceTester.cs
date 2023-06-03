using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SullysToolkit
{
    public class GameBoardAndGamePieceTester : MonoBehaviour
    {
        //Delcarations
        [Header("Command Settings")]
        [SerializeField] private int _xPosition;
        [SerializeField] private int _yPosition;
        [SerializeField] private int _xMoveDirection;
        [SerializeField] private int _yMoveDirection;
        [SerializeField] private GamePiece _targetSelection;
        [SerializeField] private List<GamePiece> _countedPiecesOnPosition;


        [Header("Testing Commands")]
        [SerializeField] private bool _addNewUnitToBoard;
        [SerializeField] private bool _addNewTerrainToBoard;
        [SerializeField] private bool _addNewPOIToBoard;
        [SerializeField] private bool _removeSelectionFromGameboard;
        [SerializeField] private bool _addSelectionToGameboard;
        [SerializeField] private bool _countPiecesOnPosition;

        [Header("Composition Testing Commands")]
        [SerializeField] private bool _moveSelectionInDirection;



        [Header("References")]
        [SerializeField] private GameBoard _gameBoardReference;
        [SerializeField] private Transform _bagOfHoldingReference;

        [SerializeField] private GamePiece _unitPiecePrefab;
        [SerializeField] private GamePiece _terrainPiecePrefab;
        [SerializeField] private GamePiece _POIPiecePrefab;



        //Monobehaviours
        private void Update()
        {
            ListenForDebugCommands();
        }



        //Internal Utils
        private void ListenForDebugCommands()
        {
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

            if (_moveSelectionInDirection)
            {
                _moveSelectionInDirection = false;
                if (_targetSelection!= null)
                    MovePiece(_targetSelection);
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

            if (moveablePiece!= null)
                moveablePiece.MoveToNeighborCell((_xPosition, _yPosition));
        }

        //Getters, Setters, & Commands
        //...




    }
}

