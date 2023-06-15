using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SullysToolkit
{
    public class GamePieceEventResponder : MonoBehaviour
    {
        //Declarations
        private GamePieceDataDisplayController _displayControllerReference;
        //Make any displayableStat classes un/subscribe to this class


        //Monobehaviours
        private void Awake()
        {
            _displayControllerReference = GetComponent<GamePieceDataDisplayController>();
        }

        private void OnEnable()
        {
            SubscribeToGamePieceEvents();
        }

        private void OnDisable()
        {
            UnsubscribeToGamePieceEvnets();
        }



        //Internal Utils
        private void SubscribeToGamePieceEvents()
        {

        }

        private void UnsubscribeToGamePieceEvnets()
        {

        }

        private void UpdateGamePieceDisplay()
        {
            if (_displayControllerReference != null)
                _displayControllerReference.UpdateData();
        }



        //Getters, Setters, & Commands
        //...




    }
}

