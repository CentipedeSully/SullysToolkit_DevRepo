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
            SetupRaycasterOnStart();
        }


        //Internal Utils
        private void SetupRaycasterOnStart()
        {
            _mouseRaycasterTool.SetSelectionCache(this);
            ChangeSelectionFilterToDetectUnits();
        }

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

        private void MakeSelectedGamePieceInteractWithOtherGamePiece(GamePiece otherGamePiece)
        {
            IInteractablePiece interactablePiece = otherGamePiece.GetComponent<IInteractablePiece>();

            if (interactablePiece != null && _selectedGamePiece != null)
            {
                if (interactablePiece.GetGamePiece() != _selectedGamePiece)
                    interactablePiece.TriggerInteractionEvent(_selectedGamePiece);
            }
                
        }

        private void MoveSelectedPieceToSelectedAdjacentTerrain()
        {
            //Get movedata
            IMoveablePiece moveablePiece = _selectedGamePiece?.GetComponent<IMoveablePiece>();

            STKDebugLogger.LogStatement(_isDebugActive, $"Attempting to Move Selected GamePiece {_selectedGamePiece} to selected Terrain {_selectedTerrainPosition}...");

            //Validate TerrainPositioning
            if (moveablePiece != null && _selectedTerrainPosition != null)
            {
                if (IsSelectedTerrainAdjacentToSelectedGamePiece())
                {
                    int xDirection = _selectedTerrainPosition.GetGridPosition().Item1 - _selectedGamePiece.GetGridPosition().Item1;
                    int yDirection = _selectedTerrainPosition.GetGridPosition().Item2 - _selectedGamePiece.GetGridPosition().Item2;
                    moveablePiece.MoveToNeighborCell((xDirection,yDirection));
                }
                    
                else
                    STKDebugLogger.LogStatement(_isDebugActive, $"selected terrain and gamePiece not adjacent. Ignoring Move Command");
            }
                
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

        private GamePiece GetUnitOnSelectedTerrain()
        {
            int xPosition = _selectedTerrainPosition.GetGridPosition().Item1;
            int yPosition = _selectedTerrainPosition.GetGridPosition().Item2;
            List<GamePiece> gamePiecesOnSelectedPosition = _gameBoard.GetPiecesOnPosition((xPosition, yPosition));
            STKDebugLogger.LogStatement(_isDebugActive, $"Checking Selected Terrain ({xPosition},{yPosition}) for Occupancy...");

            IEnumerable<GamePiece> unitOnPosition =
                from piece in gamePiecesOnSelectedPosition
                where piece.GetGamePieceType() == GamePieceType.Unit
                select piece;

            if (unitOnPosition.Any())
            {
                STKDebugLogger.LogStatement(_isDebugActive, $"Found a Unit at position ({xPosition},{yPosition}): {unitOnPosition.First()}");
                return unitOnPosition.First();
            }
            else
            {
                STKDebugLogger.LogStatement(_isDebugActive, $"No Unit found on position ({xPosition},{yPosition})");
                return null;
            }

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



        /*
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
                        MakeSelectedGamePieceInteractWithOtherGamePiece(unitOnPosition.First());
                    }
                        

                    else if (pointOfInterestOnPosition.Any())
                    {
                        STKDebugLogger.LogStatement(_isDebugActive, $"Found a Point Of Interest to interact with: {pointOfInterestOnPosition.First()}");
                        MoveSelectedPieceToSelectedAdjacentTerrain();
                        MakeSelectedGamePieceInteractWithOtherGamePiece(pointOfInterestOnPosition.First());
                    }
                        

                    else if (terrainOnPosition.Any())
                    {
                        STKDebugLogger.LogStatement(_isDebugActive, $"Found only Terrain here: {terrainOnPosition.First()}");
                        MoveSelectedPieceToSelectedAdjacentTerrain();
                    }
                        
                }
                else
                    STKDebugLogger.LogStatement(_isDebugActive, $"Aborting Interaction. Cell {xTargetCell},{yTargetCell} isn't On the grid");
            }
        }
        */


        public GameObject GetSelection()
        {
            return _selectedGamePiece.gameObject;
        }

        public void SetSelection(GameObject newSelection)
        {
            if (_isSelectorReady)
            {
                if (newSelection != null)
                {
                    GamePiece gamePiece = newSelection.GetComponent<GamePiece>();

                    if (gamePiece != null)
                    {
                        if (gamePiece.GetGamePieceType() == GamePieceType.Unit)
                        {
                            STKDebugLogger.LogStatement(_isDebugActive,$"First Selection Made (Unit): {gamePiece}");
                            _selectedGamePiece = gamePiece;
                            ChangeSelectionFilterToDetectTerrain();
                            CooldownSelection();
                        }

                        //this can only happen if we've already selected a unit. The selection filter changes when a unit is selected,
                        //which makes selecting a unit a second time impossible.
                        else if (gamePiece.GetGamePieceType() == GamePieceType.Terrain)
                        {
                            STKDebugLogger.LogStatement(_isDebugActive, $"Second Selection Made (Terrain): {gamePiece}");
                            _selectedTerrainPosition = gamePiece;

                            if (IsSelectedTerrainAdjacentToSelectedGamePiece())
                            {
                                GamePiece foundUnit = GetUnitOnSelectedTerrain();

                                if (foundUnit != null && foundUnit != _selectedGamePiece)
                                    MakeSelectedGamePieceInteractWithOtherGamePiece(foundUnit);

                                else if (foundUnit == null)
                                    MoveSelectedPieceToSelectedAdjacentTerrain();
                            }

                            STKDebugLogger.LogStatement(_isDebugActive, $"Command Execution Completed. Selector Reset");
                            ClearSelection();
                            ChangeSelectionFilterToDetectUnits();
                            CooldownSelection();
                        }

                        else
                        {
                            STKDebugLogger.LogStatement(_isDebugActive, $"Selection Miss Detected. Selector Reset");
                            ClearSelection();
                            ChangeSelectionFilterToDetectUnits();
                        }
                    }
                }
                else
                {
                    STKDebugLogger.LogStatement(_isDebugActive, $"Selected gameObject has no GamePiece Component. Selector Reset");
                    ClearSelection();
                    ChangeSelectionFilterToDetectUnits();
                }
            }

            //STKDebugLogger.LogStatement(_isDebugActive, $"Selector Still Cooling down to avoid undesired multiclicking. Please Wait...");



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

