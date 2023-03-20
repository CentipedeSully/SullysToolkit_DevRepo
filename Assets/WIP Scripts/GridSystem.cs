using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SullysToolkit
{
    public class GridSystem<T>
    {
        //Delcarations
        private T[,] _gridData;

        public int Width { get; }

        public int Height { get; }

        public float CellSize { get; }

        public Vector3 Origin { get; }

        
        //Constructor
        public GridSystem(int x, int y, float cellSize, Vector3 origin, T defaultValue = default)
        {
            this.Width = Mathf.Max(1,x);
            this.Height = Mathf.Max(1, y); ;
            this.CellSize = Mathf.Max(.1f, cellSize);
            this.Origin = origin;

            _gridData = new T[Width, Height];
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                    _gridData[i, j] = defaultValue;
            }
        }


        //Utils
        public bool IsCellInGrid(int x, int y)
        {
            if ((x >= 0 && x < Width) && (y >= 0 && y < Height))
                return true;
            else return false;
        }

        public T GetValueAtCell(int x, int y)
        {
            if (IsCellInGrid(x, y))
                return _gridData[x, y];
            else
            {
                Debug.LogError($"{this} Error: CellOutOfBounds. Cant get position of cell. returning default value");
                return default;
            }
        }

        public void SetValueAtCell(int x, int y, T value)
        {
            if (IsCellInGrid(x, y))
                _gridData[x, y] = value;
        }

        public Vector3 GetPositionFromCell(int x, int y)
        {
            if (IsCellInGrid(x,y))
                return new Vector3( (x * CellSize) - (CellSize/2) , (y * CellSize) - (CellSize/2), 0) ;

            else
            {
                Debug.LogError($"{this} Error: CellOutOfBounds. Cant get position of cell. returning zero");
                return Vector3.zero;
            }
        }

        public bool IsPositionOnGrid(Vector3 position)
        {
            //establish grid bounds
            float minX = 0;
            float maxX = Width * CellSize;
            float minY = 0;
            float maxY = Height * CellSize;

            //return if position beyond bounds
            if (position.x > maxX || position.x < minX || position.y > maxY || position.y < minY)
                return false;

            else return true;
        }

        public (int x, int y) GetCellFromPosition(Vector3 position)
        {
            if ( IsPositionOnGrid(position))
            {
                int xCellPositon = Mathf.FloorToInt(position.x / CellSize);
                int yCellPosition = Mathf.FloorToInt(position.y / CellSize);
                return (xCellPositon, yCellPosition);
            }

            else
            {
                Debug.LogError($"{this} Error: Position {position} not on Grid. Returning (-1,-1) cell position");
                return (-1, -1);
            }
            


        }

        
    }

}

