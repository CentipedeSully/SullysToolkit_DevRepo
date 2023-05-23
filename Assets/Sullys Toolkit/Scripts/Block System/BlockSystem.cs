using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace SullysToolkit
{
    public class Block
    {
        //Declarations
        [SerializeField] protected int _width = 1;
        [SerializeField] protected int _height = 1;
        [SerializeField] protected string _name = "Unnamed Block";
        [SerializeField] protected Block _parent;
        [SerializeField] protected List<Block> _children;
        [SerializeField] protected int _maxChildren;



        //Constructors
        public Block(int width, int height, string name, Block parent, int maxChildren)
        {
            this._width = width;
            this._height = height;
            this._name = name;
            this._parent = parent;
            this._maxChildren = maxChildren;
        }


        //Internal Utils
        //...



        //Getters & Setters
        public int GetWidth()
        {
            return _width;
        }

        public int GetHeight()
        {
            return _height;
        }

        public string GetName()
        {
            return _name;
        }

        public Block GetParent()
        {
            return _parent;
        }

        public List<Block> GetChildren()
        {
            return _children;
        }

        public int GetMaxChildren()
        {
            return _maxChildren;
        }

        //public abstract void RotateDimensions();
    }


    public class BlockSystem : MonoBehaviour
    {
        //Declarations
        [Header("Init Settings")]
        [SerializeField] private int _gridWidth;
        [SerializeField] private int _gridHeight;
        [SerializeField] private float _cellSize;
        [SerializeField] private Vector3 _gridBottomLeftCorner;
        private GridSystem<Block> _blockGrid;



        //Monobehaviours
        private void Awake()
        {
            _blockGrid = new GridSystem<Block>(_gridWidth, _gridHeight, _cellSize, _gridBottomLeftCorner, () => new Block(_gridWidth, _gridHeight, "unnamed block", null, 1));
            _blockGrid.SetDebugDrawing(true);
            Debug.Log(_blockGrid.GetValueAtCell(0, 0));
        }



        //Interface Utils




        //Utils




        //Debugging




    }


}

