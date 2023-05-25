using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SullysToolkit
{

    public class Block2D : MonoBehaviour
    {
        //Declarations
        [SerializeField] private int _relativeX;
        [SerializeField] private int _relativeY;

        private BlockSystem2D _parentBlockSystem;
        [SerializeField] private GridSystem<bool> _boolGrid;




        //Monobehaviours
        private void Update()
        {
            if (_boolGrid!= null)
                Debug.Log( "IsBlockOverAvailableSpace: " + IsBlockOverAvailableSpace(_boolGrid));
        }





        //Internal Utils






        //Getters, Setters, and Commands
        public BlockSystem2D GetParentBlockSystem()
        {
            return _parentBlockSystem;
        }

        public void SetParentBlockSystem(BlockSystem2D newParent)
        {
            _parentBlockSystem = newParent;
        }

        public int GetRelativeX()
        {
            return _relativeX;
        }

        public void SetRelativeX(int newPosition)
        {
            _relativeX = newPosition;
        }

        public int GetRelativeY()
        {
            return _relativeY;
        }

        public void SetRelativeY(int newPosition)
        {
            _relativeY = newPosition;
        }

        public GridSystem<bool> GetGrid()
        {
            return _boolGrid;
        }

        public void SetGrid(GridSystem<bool> newGrid)
        {
            _boolGrid = newGrid;
        }

        public bool IsBlockOverAvailableSpace(GridSystem<bool> grid)
        {
            (int, int) currentGridPosition;

            if (grid.IsPositionOnGrid(transform.position))
            {
                currentGridPosition = grid.GetCellFromPosition(transform.position);
                Debug.Log($"BlockIndex({_relativeX},{_relativeY}), Current grid Position: " + currentGridPosition);

                return grid.GetValueAtCell(currentGridPosition.Item1,currentGridPosition.Item2) == false;
            }

            return false;
        }
        


        //Debugging




    }
}


