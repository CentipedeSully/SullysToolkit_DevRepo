using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace SullysToolkit
{
    public class BlockCollection : MonoBehaviour
    {
        //Declarations
        [SerializeField] private string _collectionName;
        [SerializeField] private List<GameObject> _prefabCollection;




        //Monobehaviors





        //Internal Utils





        //Getters, Setters, & Commands
        public List<GameObject> GetCollection()
        {
            return _prefabCollection;
        }

        public void AddNewPrefab(GameObject newPrefab)
        {
            if (newPrefab != null)
                _prefabCollection.Add(newPrefab);
        }



    }
}

