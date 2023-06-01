using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SullysToolkit
{
    public enum GameBoardLayer
    {
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
        [SerializeField] private float _cellSize;
        [SerializeField] private List<GamePiece> _heldGamePieces;

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
            _heldGamePieces = new List<GamePiece>();
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

        public List<GamePiece> GetAllGamePieces()
        {
            return _heldGamePieces;
        }

        public List<GamePiece> GetPiecesInLayer(GameBoardLayer layer)
        {

            List<GamePiece> piecesInRequestedLayer = new List<GamePiece>();
            foreach (GamePiece gamePiece in _heldGamePieces)
            {
                if (gamePiece.GetBoardLayer() == layer)
                    piecesInRequestedLayer.Add(gamePiece);
            }

            return piecesInRequestedLayer;
        }

        public void AddGamePiece(GamePiece newGamePiece, GameBoardLayer deseiredLayer, (int, int) xyDesiredPosition)
        {
            //
        }

        public bool IsPositionOccupied((int,int) xyPosition, GameBoardLayer layer)
        {
            return true;
            List<GamePiece> piecesInSpecifiedLayer = GetPiecesInLayer(layer);
            foreach (GamePiece gamePiece in piecesInSpecifiedLayer)
            {
                if (gamePiece.GetGridPosition() == xyPosition)
                {

                }
            }
        }

    }
}

