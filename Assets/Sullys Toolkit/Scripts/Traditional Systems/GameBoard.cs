using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SullysToolkit
{
    public enum GameBoardLayer
    {
        Undefined,
        Units,
        PointsOfInterest,
        Terrain
    }

    public class GameBoard : MonoBehaviour
    {
        //Declarations
        [Header("Board Size")]
        [SerializeField] [Min(1)] private int _rows = 1;
        [SerializeField] [Min(1)] private int _columns = 1;
        [SerializeField] [Min(.1f)] private float _cellSize = .1f;
        [SerializeField] private List<GamePiece> _gamePiecesInPlay;

        private GridSystem<bool> _boardGrid;




        //Monobehaviours
        private void Awake()
        {
            InitializeGamePieceList();
            CreateBoardGrid();   
        }




        //Internal Utils
        private void CreateBoardGrid()
        {
            Vector3 boardOrigin = new Vector3(transform.position.x - (_columns * _cellSize) / 2, transform.position.y - _rows * _cellSize / 2, transform.position.z);
            _boardGrid = new GridSystem<bool>(_columns, _rows, _cellSize, boardOrigin, () => { return false; });
            _boardGrid.SetDebugDrawDuration(999);
            _boardGrid.SetDebugDrawing(true);
        }

        private void InitializeGamePieceList()
        {
            _gamePiecesInPlay = new List<GamePiece>();
        }

        private void SetGamePieceAsChild(GamePiece gamePiece)
        {
            gamePiece.transform.SetParent(this.transform);
        }


        //Getters, Setters, & Commands
        public GridSystem<bool> GetGrid()
        {
            return _boardGrid;
        }

        public int GetRowCount()
        {
            return _rows;
        }

        public int GetColumnCount()
        {
            return _columns;
        }

        public float GetCellSize()
        {
            return _cellSize;
        }

        public List<GamePiece> GetAllGamePiecesInPlay()
        {
            return _gamePiecesInPlay;
        }

        public List<GamePiece> GetPiecesInLayer(GameBoardLayer layer)
        {

            List<GamePiece> specifiedGamePiecesList =
                (from gamePiece in _gamePiecesInPlay
                where gamePiece.GetBoardLayer() == layer
                select gamePiece).ToList();

            return specifiedGamePiecesList;
        }

        public void AddGamePiece(GamePiece newGamePiece, GameBoardLayer deseiredLayer, (int, int) xyDesiredPosition)
        {
            bool _doesPieceAlreadyExistOnBoard = DoesGamePieceExistOnBoard(newGamePiece);
            bool _isPositionAlreadyOccupiedOnLayer = IsPositionOccupied(xyDesiredPosition, deseiredLayer);
            
            if (!_doesPieceAlreadyExistOnBoard && !_isPositionAlreadyOccupiedOnLayer)
            {
                
                newGamePiece.SetGameBoard(this);
                newGamePiece.SetBoardLayer(deseiredLayer);
                newGamePiece.SetGridPosition(xyDesiredPosition);
                SetGamePieceAsChild(newGamePiece);

                if (newGamePiece.gameObject.activeSelf == false)
                    newGamePiece.gameObject.SetActive(true);

                _gamePiecesInPlay.Add(newGamePiece);
            }
        }

        public void RemoveGamePieceFromBoard(GamePiece gamePiece)
        {
            if (_gamePiecesInPlay.Contains(gamePiece))
            {
                _gamePiecesInPlay.Remove(gamePiece);
                gamePiece.RemoveFromPlay();
            }
        }

        public bool IsPositionOccupied((int,int) xyPosition, GameBoardLayer layer)
        {
            List<GamePiece> possiblePieces = GetPiecesInLayer(layer);

            var occupancyQuery =
                from gamePiece in possiblePieces
                where gamePiece.GetGridPosition() == xyPosition
                select gamePiece;

            if (occupancyQuery.Count() > 0)
                return false;
            else return true;
        }

        public bool DoesGamePieceExistOnBoard(GamePiece gamePiece)
        {
            return _gamePiecesInPlay.Contains(gamePiece);
        }

    }
}

