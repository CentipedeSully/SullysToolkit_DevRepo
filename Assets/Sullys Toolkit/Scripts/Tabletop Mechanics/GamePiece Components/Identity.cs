using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SullysToolkit
{
    public class Identity : MonoBehaviour, IIdentityDefinition
    {
        //Declarations
        [SerializeField] private GamePieceType _type;
        [SerializeField] private string _name = "Unnamed Piece";
        [SerializeField] private string _faction = "Independent";
        [SerializeField] private bool _isHostile = false;
        private GamePiece _gamePieceReference;


        //Monobehaviours
        private void Awake()
        {
            InitializeReferences();
        }


        //Internal Utils
        private void InitializeReferences()
        {
            _gamePieceReference = GetComponent<GamePiece>();
        }


        //Getters, Setters, & Commands
        public string GetFaction()
        {
            return _faction;
        }

        public GamePiece GetGamePiece()
        {
            return _gamePieceReference;
        }

        public string GetName()
        {
            return _name;
        }

        public bool IsHostile()
        {
            return _isHostile;
        }

        public void SetFaction(string newFaction)
        {
            if (newFaction != null)
                _faction = newFaction;
        }

        public void SetHostility(bool value)
        {
            _isHostile = value;
        }

        public void SetName(string newName)
        {
            if (newName != null)
                _name = newName;
        }

        GamePieceType IIdentityDefinition.GetType()
        {
            return _type;
        }
    }
}

