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
        [SerializeField] private GamePiece _targetSelection;


        [Header("Testing Commands")]
        [SerializeField] private bool _addNewUnitToBoard;
        [SerializeField] private bool _addNewTerrainToBoard;
        [SerializeField] private bool _addNewPOIToBoard;
        [SerializeField] private bool _removeSelectionFromGameboard;
        [SerializeField] private bool _addSelectionToGameboard;


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

        //Getters, Setters, & Commands
        //...




    }
}

