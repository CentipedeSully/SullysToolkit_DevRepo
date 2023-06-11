using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SullysToolkit
{
    public class GamePieceController : MonoBehaviour, ITurnListener, ISelectionCache<GameObject>
    {
        //Declarations
        [Header("Controller Settings")]
        [SerializeField] private TurnPhase _controlsUnlockedPhase;
        [SerializeField] private bool _isControlAvailable = false;

        [Header("Selection Settings")]
        [SerializeField] private GamePiece _selectedGamePiece;
        [SerializeField] private GamePiece _selectedTerrainPosition;
        [Space(20)]

        [SerializeField] private bool _isTargetingOnlyTerrain;
        [SerializeField] private bool _isTargetingOnlyUnits;
        [SerializeField] private LayerMask _unitSelectionMask;
        [SerializeField] private LayerMask _terrainSelectionMask;
        [SerializeField] private bool _isSelectorReady = true;
        [SerializeField] private float _selectionCooldown = .1f;



        [Header("References")]
        [SerializeField] private TurnSystem _turnSystem;
        [SerializeField] private GameBoard _gameBoard;
        [SerializeField] private MouseRaycaster _mouseRaycasterTool;

        [Header("Debugging Utils")]
        [SerializeField] private bool _isDebugActive = false;




        //Monobehaviours
        private void Start()
        {
            _mouseRaycasterTool.SetSelectionCache(this);
            ChangeSelectionFilterToDetectUnits();
        }


        //Internal Utils
        private bool IsSelectedTerrainAdjacentToSelectedGamePiece()
        {
            STKDebugLogger.LogStatement(_isDebugActive, $"Checking if Selected Terrain and Selected Gamepiece are adjacent");
            if (_selectedGamePiece != null && _selectedTerrainPosition != null)
            {
                int xDifference = 0; 
                int yDifference = 0;

                xDifference = _selectedTerrainPosition.GetGridPosition().Item1 - _selectedGamePiece.GetGridPosition().Item1;
                yDifference = _selectedTerrainPosition.GetGridPosition().Item2 - _selectedGamePiece.GetGridPosition().Item2;

                STKDebugLogger.LogStatement(_isDebugActive, $"Absolute Distance Btwn Selected GamePiece and Terrain: {xDifference},{yDifference}\n" +
                    $"Is Selected Terrain and Selected GameObject Adjacent: {Mathf.Abs(xDifference) < 2 && Mathf.Abs(yDifference) < 2}");

                return (Mathf.Abs(xDifference) < 2 && Mathf.Abs(yDifference) < 2);

            }
            else
            {
                STKDebugLogger.LogStatement(_isDebugActive, $"Unable to determine adjacency. One of the Selections is null");
                return false;
            }
            
        }

        private void InteractWithGamePiece(GamePiece gamePiece)
        {
            IInteractablePiece interactablePiece = gamePiece.GetComponent<IInteractablePiece>();

            if (interactablePiece != null && interactablePiece.GetGamePiece() != _selectedGamePiece)
                interactablePiece.TriggerInteractionEvent(_selectedGamePiece);
        }

        private void MoveSelectedPieceToSelectedTerrain()
        {
            //Get movedata
            IMoveablePiece moveablePiece = _selectedGamePiece?.GetComponent<IMoveablePiece>();

            STKDebugLogger.LogStatement(_isDebugActive, $"Attempting to Move Selected GamePiece {_selectedGamePiece} to selected Terrain {_selectedTerrainPosition}...");

            //Validate TerrainPositioning
            if (moveablePiece != null && _selectedTerrainPosition != null)
                moveablePiece.MoveToNeighborCell(_selectedTerrainPosition.GetGridPosition());
            else
                STKDebugLogger.LogStatement(_isDebugActive, $"Move Aborted due to null selection.");
        }

        private void CooldownSelection()
        {
            _isSelectorReady = false;
            Invoke("ReadySelector", _selectionCooldown);
        }

        private void ReadySelector()
        {
            _isSelectorReady = true;
        }


        //Getters, Setters, & Commands
        public bool IsDebugActive()
        {
            return _isDebugActive;
        }

        public bool IsControlAvailable()
        {
            return _isControlAvailable;
        }

        public int GetResponsePhase()
        {
            return (int)_controlsUnlockedPhase;
        }

        public bool IsTurnListenerReadyToPassPhase()
        {
            return true;
        }

        public void RespondToNotification(int turnNumber)
        {
            _isControlAvailable = true;
        }

        public void ResetResponseFlag()
        {
            _isControlAvailable = false;
        }

        public ITurnBroadcaster GetTurnBroadcaster()
        {
            return _turnSystem;
        }

        public string GetConcreteListenerNameForDebugging()
        {
            return ToString() + ", ID:" + GetInstanceID();
        }

        public void ResetUtilsOnTurnSystemInterruption()
        {
            _isControlAvailable = false;
        }

        public void ClearSelection()
        {
            _selectedGamePiece = null;
            _selectedTerrainPosition = null;
        }

        public void ChangeSelectionFilterToDetectUnits()
        {
            _isTargetingOnlyUnits = true;
            _mouseRaycasterTool.SetSelectableLayers(_unitSelectionMask);

            if (_isTargetingOnlyTerrain)
                _isTargetingOnlyTerrain = false;
        }

        public void ChangeSelectionFilterToDetectTerrain()
        {
            _isTargetingOnlyTerrain = true;
            _mouseRaycasterTool.SetSelectableLayers(_terrainSelectionMask);

            if (_isTargetingOnlyUnits)
                _isTargetingOnlyUnits = false;
        }

        public void InteractWithNeighborOrMoveToNeighbor((int, int) xyDirection)
        {
            if (_gameBoard != null && _selectedGamePiece != null)
            {

                int xTargetCell = xyDirection.Item1 + _selectedGamePiece.GetGridPosition().Item1;
                int yTargetCell = xyDirection.Item2 + _selectedGamePiece.GetGridPosition().Item2;

                STKDebugLogger.LogStatement(_isDebugActive, $"Attempting Interaction Command onto Cell {xTargetCell},{yTargetCell}...");

                if (_gameBoard.GetGrid().IsCellInGrid(xTargetCell, yTargetCell))
                {
                    List<GamePiece> gamePiecesOnTargetPosition = _gameBoard.GetPiecesOnPosition((xTargetCell, yTargetCell));

                    IEnumerable<GamePiece> unitOnPosition =
                        from piece in gamePiecesOnTargetPosition
                        where piece.GetGamePieceType() == GamePieceType.Unit
                        select piece;

                    IEnumerable<GamePiece> pointOfInterestOnPosition =
                        from piece in gamePiecesOnTargetPosition
                        where piece.GetGamePieceType() == GamePieceType.PointOfInterest
                        select piece;

                    IEnumerable<GamePiece> terrainOnPosition =
                        from piece in gamePiecesOnTargetPosition
                        where piece.GetGamePieceType() == GamePieceType.Terrain
                        select piece;

                    if (unitOnPosition.Any())
                    {
                        STKDebugLogger.LogStatement(_isDebugActive, $"Found a Unit to Interact With: {unitOnPosition.First()}");
                        InteractWithGamePiece(unitOnPosition.First());
                    }
                        

                    else if (pointOfInterestOnPosition.Any())
                    {
                        STKDebugLogger.LogStatement(_isDebugActive, $"Found a Point Of Interest to interact with: {pointOfInterestOnPosition.First()}");
                        InteractWithGamePiece(pointOfInterestOnPosition.First());
                    }
                        

                    else if (terrainOnPosition.Any())
                    {
                        STKDebugLogger.LogStatement(_isDebugActive, $"Found only Terrain here: {terrainOnPosition.First()}");
                        //InteractWithGamePiece(terrainOnPosition.First());
                        MoveSelectedPieceToSelectedTerrain();
                    }
                        
                }
                else
                    STKDebugLogger.LogStatement(_isDebugActive, $"Aborting Interaction. Cell {xTargetCell},{yTargetCell} isn't On the grid");
            }
        }



        public GameObject GetSelection()
        {
            return _selectedGamePiece.gameObject;
        }

        public void SetSelection(GameObject newSelection)
        {
            if (_isSelectorReady)
            {
                CooldownSelection();
                if (newSelection != null)
                {
                    GamePiece gamePiece = newSelection.GetComponent<GamePiece>();

                    if (gamePiece != null)
                    {
                        if (gamePiece.GetGamePieceType() == GamePieceType.Unit)
                        {
                            _selectedGamePiece = gamePiece;
                            ChangeSelectionFilterToDetectTerrain();
                        }

                        //this can only happen if we've already selected a unit. The selection filter changes when a unit is selected,
                        //which makes selecting a unit a second time impossible.
                        else if (gamePiece.GetGamePieceType() == GamePieceType.Terrain && _isTargetingOnlyTerrain == true)
                        {
                            _selectedTerrainPosition = gamePiece;

                            InteractWithNeighborOrMoveToNeighbor(gamePiece.GetGridPosition());
                            ClearSelection();
                            ChangeSelectionFilterToDetectUnits();
                        }

                        else
                        {
                            ClearSelection();
                            ChangeSelectionFilterToDetectUnits();
                        }
                    }
                }
                else
                {
                    ClearSelection();
                    ChangeSelectionFilterToDetectUnits();
                }
            }
            
                
            
        }

        public List<GameObject> GetSelectionCollection()
        {
            if (_selectedGamePiece == null)
                return new List<GameObject>();
            else
            {
                List<GameObject> returnList = new List<GameObject>();
                returnList.Add(_selectedGamePiece.gameObject);

                return returnList;
            }
        }

        public void AddSelection(GameObject newSelection)
        {
            SetSelection(newSelection);
        }

        public void RemoveSelection(GameObject existingSelection)
        {
            ClearSelection();
            ChangeSelectionFilterToDetectUnits();
        }

        public bool IsSelectionAvailable()
        {
            return _selectedGamePiece != null;
        }
    }
}

