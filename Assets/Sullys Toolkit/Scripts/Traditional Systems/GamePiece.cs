using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SullysToolkit
{

    public class GamePiece : MonoBehaviour
    {
        //Declarations
        [SerializeField] private GameBoard _gameBoard;
        [SerializeField] private GameBoardLayer _boardLayer;
        [SerializeField] private (int, int) _currentGridPosition = (-1,-1);




        //Monobehaviours





        //Internal Utils
        




        //Getters, Setters, & Commands
        public GameBoard GetGameBoard()
        {
            return _gameBoard;
        }

        public void SetGameBoard(GameBoard newBoard)
        {
            if (newBoard != null)
                _gameBoard = newBoard;
        }

        public GameBoardLayer GetBoardLayer()
        {
            return _boardLayer;
        }

        public void SetBoardLayer(GameBoardLayer newLayer)
        {
            _boardLayer = newLayer;
        }

        public (int,int) GetGridPosition()
        {
            return _currentGridPosition;
        }

        public void SetGridPosition((int,int) newPosition)
        {
            if (_gameBoard.GetGrid().IsCellInGrid(newPosition.Item1, newPosition.Item2))
                _currentGridPosition = newPosition;
        }

    }
}

