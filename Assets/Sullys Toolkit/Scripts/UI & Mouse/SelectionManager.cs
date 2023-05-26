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
        [SerializeField] private bool _isDegubActive = false;



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
                if (!_selectionList.Contains(newSelection))
                {
                    _selectionList.Add(newSelection);
                    if (_isDegubActive)
                        Debug.Log("Added new object to the 'selection' Collection");
                }
                if (_isDegubActive)
                    Debug.Log("Object Already exists in collection");

            }

        }

        public void ClearSelection()
        {
            _selectionList = new List<GameObject>();
            if (_isDegubActive)
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
                    if (_isDegubActive)
                        Debug.Log("object removed from selection collection");
                }

                else
                    if (_isDegubActive)
                    Debug.Log("requested object doesn't exist in selection collection. Ignoring remove command");
            }

        }

        public void SetSelection(GameObject newSelection)
        {
            if (newSelection != null)
            {
                _selectionList = new List<GameObject>();
                _selectionList.Add(newSelection);

                if (_isDegubActive)
                    Debug.Log("selection overridden successfully");
            }
        }




        //Internal Utils
        //...



        //Getters, Setters, & Commands
        public bool IsDebugActive()
        {
            return _isDegubActive;
        }

        public void SetDebugMode(bool newValue)
        {
            _isDegubActive = newValue;
        }



        //Debugging
        //...

    }
}
