using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SullysToolkit
{
    public class InteractorBehavior : MonoBehaviour, IInterationPerformerPiece
    {
        //Declarations
        [Header("Interactor Settings")]
        [SerializeField] private GamePiece _gamePieceRef;


        //Monobehaviours
        private void Awake()
        {
            _gamePieceRef = GetComponent<GamePiece>();
        }


        //Internal Utils



        //Getters, Setters, & Commands
        public GamePiece GetGamePiece()
        {
            return _gamePieceRef;
        }

        public void InteractWithPointOfInterest(IInteractablePiece gamePiece)
        {
            //
        }
    }
}

