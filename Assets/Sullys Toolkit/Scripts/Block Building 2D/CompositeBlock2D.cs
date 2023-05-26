using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace SullysToolkit
{

    public class CompositeBlock2D : MonoBehaviour
    {
        //Declarations
        private bool _isInitializationComplete = false;
        private float _childrenBlockSize = 1;
        private List<Block2D> _childrenBlocks;

        [Header("Grid Location Settings")]
        [SerializeField] private GridCreator _gridCreator;
        [SerializeField] private bool _startCompositeOnMapOrigin = false;
        private GridSystem<bool> _grid;

        [Header("Debugging Utils")]
        [SerializeField] private bool _isDebugActive;
        [SerializeField] private bool _rotateClockwiseCmd;
        [SerializeField] private bool _rotateCounterClockwiseCmd;




        //Monobehaviours
        private void Awake()
        {
            InitializePersonalUtilities();
        }

        private void Start()
        {
            InitializeDependentUtils();
            InitializeChildBlocksIntoSafeCollection();

            if (_startCompositeOnMapOrigin)
                SetCompositeBlockPositionToGridCenter();

            CompleteInitialization();
        }

        private void Update()
        {
            ListenForDebugCommandsIfDebugActive();
        }



        //Internal Utils
        private void InitializePersonalUtilities()
        {
            _childrenBlocks = new List<Block2D>();
        }

        private void InitializeDependentUtils()
        {
            _grid = _gridCreator.GetGrid();
            _childrenBlockSize = _gridCreator.GetCellSize();
        }

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
                    block.transform.localScale = new Vector3(_gridCreator.GetCellSize(), _gridCreator.GetCellSize(), block.transform.localScale.z);
                }
            }
        }

        private void SetCompositeBlockPositionToGridCenter()
        {
            Vector2 mapCenterCell = new Vector2(Mathf.FloorToInt(_grid.Width / 2), Mathf.FloorToInt(_grid.Height / 2));
            Vector2 mapCenterWorldCoordinates = _grid.GetPositionFromCell((int)mapCenterCell.x, (int)mapCenterCell.y);
            transform.position = new Vector3(mapCenterWorldCoordinates.x, mapCenterWorldCoordinates.y, transform.position.z);
        }

        private void CompleteInitialization()
        {
            _isInitializationComplete = true;
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

        public bool IsInitializationComplete()
        {
            return _isInitializationComplete;
        }

        public bool IsDebugActive()
        {
            return _isDebugActive;
        }

        public void SetDebugMode(bool newValue)
        {
            _isDebugActive = newValue;
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

