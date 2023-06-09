using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace SullysToolkit
{
    public class InteractionBehavior : MonoBehaviour, IInteractablePiece
    {
        //Declarations
        [Header("Interaction Settings")]
        [SerializeField] private GamePiece _gamePieceRef;

        //Events
        public delegate void InteractionEvent(InteractionBehavior subject);
        //Monobehaviours



        //Internal Utils



        //Getters, Setters, & Commands
        public GamePiece GetGamePiece()
        {
            throw new System.NotImplementedException();
        }

        public void TriggerEventOnInteraction()
        {
            throw new System.NotImplementedException();
        }
    }
}


