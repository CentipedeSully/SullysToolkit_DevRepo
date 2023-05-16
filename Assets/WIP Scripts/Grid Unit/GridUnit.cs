using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SullysToolkit;

namespace SullysToolkit
{
    public interface IAttributeReference
    {
        int GetCurrentGridMovePoints();

        void SetCurrentGridMovePoints(int newValue);

        int GetMaxGridMovePoints();

        void SetMaxGridMovePoints(int newValue);
    }

    public interface IMoveBehavior
    {
        void MoveToCell(int x, int y, int availableMovementPoints);
    }

    public abstract class GridUnit
    {
        //Declarations
        private GridSystem<GridUnit> _parentGrid;
        private (int, int) _gridPosition;
        private IAttributeReference _attributesReference;
        private GameObject _mapAvatarPrefab;
        private GameObject _currentMapAvatarInstance;



        //Constructors
        //...



        //Utils
        public void UpdateUnitGridPosition()
        {
            if (_currentMapAvatarInstance != null)
                _gridPosition = _parentGrid.GetCellFromPosition(_currentMapAvatarInstance.transform.position);
        }

        //Getters
        public GridSystem<GridUnit> GetParentGrid()
        {
            return _parentGrid;
        }

        public (int, int) GetGridPosition()
        {
            return _gridPosition;
        }

        public IAttributeReference GetUnitAttributes()
        {
            return _attributesReference;
        }

        public GameObject GetMapAvatarPrefab()
        {
            return _mapAvatarPrefab;
        }

        public GameObject GetCurrentAvatarInstance()
        {
            return _currentMapAvatarInstance;
        }



    }
}

