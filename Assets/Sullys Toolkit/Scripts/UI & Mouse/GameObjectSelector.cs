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


        //Monos






        //Internal Utils
        private void CalculateRaycastFromMousePosition()
        {
            Vector2 castOrigin = _mouseToWorld2DReference.GetWorldPosition(); 
            RaycastHit2D raycastData = Physics2D.Raycast(castOrigin, _castDirection);
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

        public void RaycastFromMousePosition()
        {

        }

    }
}

