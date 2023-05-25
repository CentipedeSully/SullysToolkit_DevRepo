using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace SullysToolkit
{
    public class BlockSystem2D : MonoBehaviour
    {
        //Declarations
        [Header("System Settings")]
        [SerializeField] private BlockSystem2D _parentConnection;
        [SerializeField] private List<BlockSystem2D> _childrenConnections;
        [SerializeField] private int _maxChildConnections;

        [Header("Block Settings")]
        [SerializeField] private float _blockSize = 1;
        [SerializeField] private List<Block2D> _blockCollection;

        [Header("TestGrid Settings")]
        [SerializeField] private GridCreator _gridCreator;
        [SerializeField] private GridSystem<bool> _grid;




        //Monobehaviours
        private void Start()
        {
            _grid = _gridCreator.GetGrid();
            _blockSize = _gridCreator.GetCellSize();
            InitializeChildBlocksIntoSafeCollection();
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
                    _blockCollection.Add(block);
                    block.SetParentBlockSystem(this);
                    block.SetGrid(_grid);
                    block.transform.localPosition = new Vector3(block.GetRelativeX() * _blockSize, block.GetRelativeY() * _blockSize, block.transform.position.z);
                }
            }
        }





        //Getters, Setters, and Commands





        //Debugging




    }

}

