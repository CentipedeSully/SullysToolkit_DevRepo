using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SullysToolkit
{
    public interface ISelectionCache<T>
    {
        T GetSelection();

        void SetSelection(T newSelection);

        List<T> GetSelectionCollection();

        void AddSelection(T newSelection);

        void RemoveSelection(T existingSelection);

        void ClearSelection();

        bool IsSelectionAvailable();
    }




    //classes
    public class SelectionManager : MonoBehaviour, ISelectionCache<GameObject>
    {
        //Declarations
        [SerializeField] private List<GameObject> _selectionList;
        [SerializeField] private bool _isMultiSelectAvaialable = false;
        [SerializeField] private bool _isDebugActive = false;



        //Monobehaviours
        private void Awake()
        {
            _selectionList = new List<GameObject>();
        }




        //Interface Utils
        public void AddSelection(GameObject newSelection)
        {
            if (newSelection != null)
            {
                if (_isMultiSelectAvaialable)
                {
                    if (!_selectionList.Contains(newSelection))
                    {
                        _selectionList.Add(newSelection);
                        if (_isDebugActive)
                            Debug.Log("Added new object to the 'selection' Collection");
                    }
                    if (_isDebugActive)
                        Debug.Log("Object Already exists in collection");
                }
                else SetSelection(newSelection);

            }

        }

        public void ClearSelection()
        {
            _selectionList = new List<GameObject>();
            if (_isDebugActive)
                Debug.Log("Selection Collection Cleared");
        }

        public GameObject GetSelection()
        {
            if (IsSelectionAvailable())
                return _selectionList[0];
            else
            {
                Debug.LogWarning($"Error in Selection Manager: Fetched a selection that doesn't exist. Return a default value...");
                return default;
            }
        }

        public List<GameObject> GetSelectionCollection()
        {
            return _selectionList;
        }

        public bool IsSelectionAvailable()
        {
            return _selectionList.Count > 0;
        }

        public void RemoveSelection(GameObject existingSelection)
        {
            if (existingSelection != null)
            {
                if (_selectionList.Contains(existingSelection))
                {
                    _selectionList.Remove(existingSelection);
                    if (_isDebugActive)
                        Debug.Log("object removed from selection collection");
                }

                else
                    if (_isDebugActive)
                    Debug.Log("requested object doesn't exist in selection collection. Ignoring remove command");
            }

        }

        public void SetSelection(GameObject newSelection)
        {
            if (newSelection != null)
            {
                _selectionList = new List<GameObject>();
                _selectionList.Add(newSelection);

                if (_isDebugActive)
                    Debug.Log("selection overridden successfully");
            }
        }




        //Internal Utils
        //...



        //Getters, Setters, & Commands
        public bool IsDebugActive()
        {
            return _isDebugActive;
        }

        public void SetDebugMode(bool newValue)
        {
            _isDebugActive = newValue;
        }



        //Debugging
        //...

    }
}
