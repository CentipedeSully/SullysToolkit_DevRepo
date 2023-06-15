using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SullysToolkit
{
    public class Identity : MonoBehaviour, IIdentityDefinition
    {
        //Declarations
        [SerializeField] private string _name = "Unnamed Piece";
        [SerializeField] private string _faction = "Independent";
        [SerializeField] private bool _isHostile = false;
        [SerializeField] private string _pieceDescription = "Undescribed Piece";
        private GamePiece _gamePieceReference;

        public delegate void IdentityEvent();
        public event IdentityEvent OnIdentityChanged;

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

        private void TriggerIndentityChangedEvent()
        {
            OnIdentityChanged?.Invoke();
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
            {
                _faction = newFaction;
                TriggerIndentityChangedEvent();
            }
        }

        public void SetHostility(bool value)
        {
            _isHostile = value;
            TriggerIndentityChangedEvent();
        }

        public void SetName(string newName)
        {
            if (newName != null)
            {
                _name = newName;
                TriggerIndentityChangedEvent();
            }
               
        }

        public string GetDescription()
        {
            return _pieceDescription;
        }

        public void SetDescription(string newDescription)
        {
            if (newDescription != null)
            {
                _pieceDescription = newDescription;
                TriggerIndentityChangedEvent();
            }
                
        }
    }
}

