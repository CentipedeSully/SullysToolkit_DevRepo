using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SullysToolkit
{
    public class GrowthBehavior : MonoBehaviour, ILevelablePiece
    {
        //Declarations
        [Header("Current Lvl Stats")]
        [SerializeField] [Min(1)] private int _currentLv = 1;
        [SerializeField] [Min(1)] private int _maxLv = 1;
        [SerializeField] [Min(0)] private int _currentExp;
        [SerializeField] [Min(0)] private int _maxExpToLvlUp;

        [Header("Growth Curves")]
        [SerializeField] private AnimationCurve _expGrowthCurve;
        [SerializeField] private AnimationCurve _healthGrowthCurve;
        [SerializeField] private AnimationCurve _atkGrowthCurve;
        [SerializeField] private AnimationCurve _defGrowthCurve;
        [SerializeField] private AnimationCurve _movePointsGrowthCurve;
        [SerializeField] private AnimationCurve _actionPointsGrowthCurve;

        [Header("Growable Stats")]
        [SerializeField] private bool _isHealthPresent;
        [SerializeField] private bool _isMovementBehaviorPresent;
        [SerializeField] private bool _isInteractionBehaviorPresent;
        [SerializeField] private bool _isConflictAttributesPresent;

        [Header("References")]
        [SerializeField] private GamePiece _gamePieceReference;
        [SerializeField] private IHealthManager _healthReference;
        [SerializeField] private IMoveablePiece _movementReference;
        [SerializeField] private IInterationPerformer _performInteractionBehaviorRef;
        //[SerializeField] private IConflictAttriutes _conflictAttributesRef;



        //Monobehaviours
        private void Awake()
        {
            InitializePersonalSettings();
        }

        private void Start()
        {
            
        }



        //Internal Utilities
        private void InitializePersonalSettings()
        {
            _gamePieceReference = GetComponent<GamePiece>();
            _healthReference = GetComponent<IHealthManager>();
            _movementReference = GetComponent<IMoveablePiece>();
            _performInteractionBehaviorRef = GetComponent<IInterationPerformer>();
            //_conflictAttributesRef = GetComponent<IConflictAttriutes>();

            if (_healthReference != null)
                _isHealthPresent = true;
            if (_movementReference != null)
                _isMovementBehaviorPresent = true;
            if (_performInteractionBehaviorRef != null)
                _isInteractionBehaviorPresent = true;
            //if (_conflictAttributesRef != null)
            //    _isConflictAttributesPresent = true;

        }

        private void InitializeOtherReferenceStats()
        {
            //IniitalizeOther stats based on ifthey exist or not
        }


        //Getters, Setters, & Commands
        public void ForceLvlUp()
        {
            throw new System.NotImplementedException();
        }

        public void GainExp(int value)
        {
            throw new System.NotImplementedException();
        }

        public AnimationCurve GetActionPointsGrowthCurve()
        {
            throw new System.NotImplementedException();
        }

        public AnimationCurve GetAtkGrowthCurve()
        {
            throw new System.NotImplementedException();
        }

        public int GetCurrentExp()
        {
            throw new System.NotImplementedException();
        }

        public int GetCurrentLevel()
        {
            throw new System.NotImplementedException();
        }

        public AnimationCurve GetDefGrowthCurve()
        {
            throw new System.NotImplementedException();
        }

        public AnimationCurve GetExpGrowthCurve()
        {
            throw new System.NotImplementedException();
        }

        public GamePiece GetGamePiece()
        {
            throw new System.NotImplementedException();
        }

        public AnimationCurve GetHealthGrowthCurve()
        {
            throw new System.NotImplementedException();
        }

        public int GetMaxLevel()
        {
            throw new System.NotImplementedException();
        }

        public AnimationCurve GetMovePointGrowthCurve()
        {
            throw new System.NotImplementedException();
        }

        public int GetNextLvlThreshold()
        {
            throw new System.NotImplementedException();
        }

        public void SetActionPointsGrowthCurve(AnimationCurve apCurve)
        {
            throw new System.NotImplementedException();
        }

        public void SetAtkGrowthCurve(AnimationCurve atkCurve)
        {
            throw new System.NotImplementedException();
        }

        public void SetCurrentExp(int value)
        {
            throw new System.NotImplementedException();
        }

        public void SetCurrentLevel(int value)
        {
            throw new System.NotImplementedException();
        }

        public void SetDefGrowthCurve()
        {
            throw new System.NotImplementedException();
        }

        public void SetExpGrowthCurve(AnimationCurve growthCurve)
        {
            throw new System.NotImplementedException();
        }

        public void SetHealthGrowthCurve(AnimationCurve growthCurve)
        {
            throw new System.NotImplementedException();
        }

        public void SetMovePointGrowthCurve(AnimationCurve mpCurve)
        {
            throw new System.NotImplementedException();
        }
    }
}

