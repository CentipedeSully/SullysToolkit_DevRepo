using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SullysToolkit
{
    public class Movement : MonoBehaviour, IMoveablePiece
    {
        //Declarations
        [Header("Movement Attributes")]
        [SerializeField] private int _currentMovePoints;
        [SerializeField] private int _maxMovePoints;

        [Header("Movement Costs")]
        [SerializeField] private int _adjacentMoveCost = 10;
        [SerializeField] private int _diagonalMoveCost = 14;


        [Header("References")]
        [SerializeField] private GamePiece _gamePieceReference;

        [Header("Debugging Utils")]
        [SerializeField] private bool _isDebugActive = false;



        //Declarations
        private void Awake()
        {
            _gamePieceReference = GetComponent<GamePiece>();
        }



        //Internal Utils
        private bool IsGamePieceInPlay()
        {
            return _gamePieceReference.IsInPlay();
        }

        private bool IsDestinationValid((int, int) xyDestination)
        {
            STKDebugLogger.LogStatement(_isDebugActive, $"Validating Move Command to Position {xyDestination.Item1},{xyDestination.Item2}...");

            if (IsGamePieceInPlay())
            {
                GameBoard gameBoard = _gamePieceReference.GetGameBoard();
                bool doesCellExistOnGrid = gameBoard.GetGrid().IsCellInGrid(xyDestination.Item1, xyDestination.Item2); ;
                bool isCellUnoccupied = gameBoard.IsPositionOccupied(xyDestination, _gamePieceReference.GetBoardLayer());

                STKDebugLogger.LogStatement(_isDebugActive, $"Validation Results for {_gamePieceReference.gameObject.name}:" +
                    $"\nDoesCellExistOnGrid: {doesCellExistOnGrid}" +
                    $"\nIsCellUnoccupied: {isCellUnoccupied}");

                return doesCellExistOnGrid && isCellUnoccupied;
            }

            else
            {
                STKDebugLogger.LogStatement(_isDebugActive, $"Validation autoFailure: GamePiece {_gamePieceReference.gameObject.name} not in play. Ignoring Move Command");
                return false;
            }

        }

        private void DecrementMovePoints()
        {
            STKDebugLogger.LogStatement(_isDebugActive, $"Decrementing MovePoints for {_gamePieceReference.gameObject.name}");
            _currentMovePoints--;
        }



        //Getters, Setters, & Commands
        public GamePiece GetGamePiece()
        {
            return _gamePieceReference;
        }

        public int GetCurrentMovePoints()
        {
            return _currentMovePoints;
        }

        public void SetCurrentMovePoints(int value)
        {
            _currentMovePoints = Mathf.Clamp(value, 0, _maxMovePoints);
        }

        public int GetMaxMovePoints()
        {
            return _maxMovePoints;
        }

        public void SetMaxMovePoints(int value)
        {
            _maxMovePoints = Mathf.Max(1, value);

            //Reflect the new maxiumum
            SetCurrentMovePoints(_currentMovePoints);
        }

        public void MoveToNeighborCell((int, int) xyDirection)
        {
            if (xyDirection.Item1 == 0 && xyDirection.Item2 == 0)
            {
                STKDebugLogger.LogStatement(_isDebugActive, $"Movement necessary for {_gamePieceReference.gameObject.name} in direction " +
                    $"{xyDirection.Item1},{xyDirection.Item2}.\n" +
                    $"Ignoring move command");

                return;
            }

            STKDebugLogger.LogStatement(_isDebugActive,$"Calculating Movement cost for {_gamePieceReference.gameObject.name} in direction " +
                $"{xyDirection.Item1},{xyDirection.Item2}...");

            //Calculate move Cost
            int xDirection = Mathf.Clamp(xyDirection.Item1, -1, 1);
            int yDirection = Mathf.Clamp(xyDirection.Item2, -1, 1);

            int moveCost;
            if (xDirection != 0 && yDirection != 0)
                moveCost = _diagonalMoveCost;
            else moveCost = _adjacentMoveCost;

            if (_currentMovePoints >= moveCost)
            {
                //Determine MoveFeasability
                int xDestination = _gamePieceReference.GetGridPosition().Item1 + xDirection;
                int yDestination = _gamePieceReference.GetGridPosition().Item2 + yDirection;
                (int, int) xyDestination = (xDestination, yDestination);
            }
            else
                STKDebugLogger.LogStatement(_isDebugActive, $"Insufficient MovePoints on {_gamePieceReference.gameObject.name} for Move Command");

        }



    }


}
