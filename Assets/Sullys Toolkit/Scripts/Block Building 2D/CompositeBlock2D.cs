using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace SullysToolkit
{

    public class CompositeBlock2D : MonoBehaviour
    {
        //Declarations
        //children block settings
        private float _childrenBlockSize = 1;
        private List<Block2D> _childrenBlocks;

        [Header("TestGrid Settings")]
        [SerializeField] private GridCreator _gridCreator;
        private GridSystem<bool> _grid;

        [Header("Debugging Utils")]
        [SerializeField] private bool _isDebugActive;
        [SerializeField] private bool _rotateClockwiseCmd;
        [SerializeField] private bool _rotateCounterClockwiseCmd;




        //Monobehaviours
        private void Start()
        {
            _grid = _gridCreator.GetGrid();
            _childrenBlockSize = _gridCreator.GetCellSize();
            _childrenBlocks = new List<Block2D>();
            InitializeChildBlocksIntoSafeCollection();
        }

        private void Update()
        {
            ListenForDebugCommandsIfDebugActive();
        }



        //Internal Utils
        private void InitializeChildBlocksIntoSafeCollection()
        {
            Block2D block;

            for (int i = 0; i < transform.childCount; i++)
            {
                block = transform.GetChild(i).GetComponent<Block2D>();

                if (block != null)
                {
                    _childrenBlocks.Add(block);
                    block.SetParentCompositeBlock2D(this);
                    block.SetGrid(_grid);
                    block.transform.localPosition = new Vector3(block.GetRelativeX() * _childrenBlockSize, block.GetRelativeY() * _childrenBlockSize, block.transform.position.z);
                    block.GetComponent<Collider2D>();
                    //Edit block collider

                }
            }
        }

        private void RotateBlockZ(float angle)
        {
            Vector3 previousRotationVector = transform.rotation.eulerAngles;
            Vector3 newRotationVector = new Vector3(previousRotationVector.x, previousRotationVector.y, previousRotationVector.z + angle);
            transform.rotation = Quaternion.Euler(newRotationVector);
        }



        //Getters, Setters, and Commands
        public void RotateBlockClockwise()
        {
            RotateBlockZ(-90);
        }

        public void RotateBlockCounterClockwise()
        {
            RotateBlockZ(90);
        }




        //Debugging
        private void ListenForDebugCommandsIfDebugActive()
        {
            if (_isDebugActive)
            {
                if (_rotateClockwiseCmd)
                {
                    _rotateClockwiseCmd = false;
                    RotateBlockClockwise();
                }

                if (_rotateCounterClockwiseCmd)
                {
                    _rotateCounterClockwiseCmd = false;
                    RotateBlockCounterClockwise();
                }
            }
        }



    }

}

