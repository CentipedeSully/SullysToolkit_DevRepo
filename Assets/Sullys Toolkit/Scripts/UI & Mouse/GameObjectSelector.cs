using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SullysToolkit
{
    public interface IMouseInteractable
    {
        void LeftClick();

        void RightClick();

        void MiddleClick();

        void OnHover();

        void OnHoverAfterDelay();

        void OnHoverExit();

    }


    public interface ISelectable
    {
        void ConfirmSelection();

        void CancelSelection();

    }


    public class GameObjectSelector : MonoBehaviour
    {
        //Declarations
        [Header("Selection Settings")]
        [SerializeField] private string _SelectorName = "Unnamed Selector";
        [SerializeField] private float _raycastDistance = 50;
        [SerializeField] private Vector3 _castDirection = Vector3.back;
        [SerializeField] private LayerMask _selectableLayers;


        [SerializeField] private GameObject _currentSelection;
        [SerializeField] private MouseToWorld2D _mouseToWorld2DReference;

        //Debugging
        [Header("Debugging Utils")]
        [SerializeField] private bool _isDebugActive;
        [SerializeField] private Color _gizmoColor;
        [SerializeField] private bool _clearSelectionCmd;


        //Monos
        private void Update()
        {
            ListenforDebugCommandsIfDebugActive();
            SetSelectionViaMousePointer();
        }

        private void OnDrawGizmosSelected()
        {
            DrawMousePointer();
        }





        //Internal Utils
        private void FindSelectedGameObjectViaRaycastFromMousePosition()
        {
            Vector2 castOrigin = _mouseToWorld2DReference.GetWorldPosition(); 
            RaycastHit2D raycastData = Physics2D.Raycast(castOrigin, _castDirection);
            if (raycastData.collider != null)
                _currentSelection = raycastData.collider.gameObject;
            
        }





        //Getters, Setters, and Commands
        public GameObject GetCurrentSelection()
        {
            return _currentSelection;
        }

        public void SetCurrentSelection(GameObject newObject)
        {
            _currentSelection = newObject;
        }

        public LayerMask GetSelectableLayers()
        {
            return _selectableLayers;
        }

        public void SetSelectableLayers(LayerMask newLayerMask)
        {
            _selectableLayers = newLayerMask;
        }

        public bool IsAnyObjectSelected()
        {
            return _currentSelection != null;
        }

        public void SetSelectionViaMousePointer()
        {
            FindSelectedGameObjectViaRaycastFromMousePosition();
        }

        public void ClearCurrentSelection()
        {
            if (_currentSelection != null)
                _currentSelection = null;
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
        private void DrawMousePointer()
        {
            Gizmos.color = _gizmoColor;
            Vector3 mouseWorldPosition = _mouseToWorld2DReference.GetWorldPosition();
            Vector3 debugLineStart = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, Camera.main.transform.position.z);
            Vector3 debugLineEnd = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, _raycastDistance);
            Gizmos.DrawLine(debugLineStart, debugLineEnd);
        }

        private void ListenforDebugCommandsIfDebugActive()
        {
            if (_isDebugActive)
            {
                if (_clearSelectionCmd)
                {
                    _clearSelectionCmd = false;
                    ClearCurrentSelection();
                }
            }
        }

    }
}

