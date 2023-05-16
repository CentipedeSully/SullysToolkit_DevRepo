using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SullysToolkit
{
    public interface IPathfinder
    {
        //BuildPath(start, end) returns a list of the path nodes

        //GetGrid

        //GetAlgorithmName() returns a string of the algorithm used


    }

    public class Node<T>
    {
        //Declarations
        private GridSystem<Node<T>> _parentGrid;

        private int xPosition = -1;

        private int yPosition = -1;

        private T value;


        //Constructor
        public Node(GridSystem<Node<T>> parent, int x, int y, T defaultValue)
        {
            this._parentGrid = parent;
            this.xPosition = x;
            this.yPosition = y;
            this.value = defaultValue;
        }



        //Utils
        public GridSystem<Node<T>> GetParentGrid()
        {
            return _parentGrid;
        }

        public int GetPositionX()
        {
            return xPosition;
        }

        public int GetPositionY()
        {
            return yPosition;
        }

        public T GetValue()
        {
            return value;
        }

        public void SetValue(T newValue)
        {
            value = newValue;
        }

    }

    public class aStarPathfinder
    {
        //Declarations
        GridSystem<Node<bool>> _pathingGrid;
        


        //Constructors



        //Interface Utils



        //Utils




    }
}

