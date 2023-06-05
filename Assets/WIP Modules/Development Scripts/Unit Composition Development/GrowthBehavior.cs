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
        [SerializeField] [Min(1)] private int _maxLv = 3;
        [SerializeField] [Min(0)] private int _currentExp;
        [SerializeField] [Min(0)] private int _nextLvExpGate;
        [SerializeField] [Min(1)] private int _baseExpGate = 10;
        [SerializeField] [Min(1)] private float _expGateGrowthMultiplier = 2;

        [Header("Growth modifiers")]
        [SerializeField] private int _healthBase = 2;
        [SerializeField] private int _healthDieSize = 4;
        [SerializeField] private int _movePointBase = 30;
        [SerializeField] private int _movePointDieSize = 12;
        [SerializeField] private int _atkBase = 2;
        [SerializeField] private int _atkGrowthDieSize = 4;
        [SerializeField] private int _defBase = 2;
        [SerializeField] private int _defGrowthDieSize = 4;
        [SerializeField] private int _damageBase = 2; 
        [SerializeField] private int _damageGrowthDieSize = 2;

        [Header("Growable Stats")]
        [SerializeField] private bool _isHealthPresent;
        [SerializeField] private bool _isMovementBehaviorPresent;
        [SerializeField] private bool _isConflictAttributesPresent;

        [Header("References")]
        [SerializeField] private GamePiece _gamePieceReference;
        [SerializeField] private IHealthManager _healthReference;
        [SerializeField] private IMoveablePiece _movementReference;
        [SerializeField] private IConflictAttributes _conflictAttributesRef;



        //Monobehaviours
        private void Awake()
        {
            InitializePersonalSettings();
        }

        private void Start()
        {
            InitializeOtherExistingGamePieceAttributes();
        }



        //Internal Utilities
        private void InitializePersonalSettings()
        {
            _gamePieceReference = GetComponent<GamePiece>();
            _healthReference = GetComponent<IHealthManager>();
            _movementReference = GetComponent<IMoveablePiece>();
            _conflictAttributesRef = GetComponent<IConflictAttributes>();

            if (_healthReference != null)
                _isHealthPresent = true;
            if (_movementReference != null)
                _isMovementBehaviorPresent = true;
            if (_conflictAttributesRef != null)
                _isConflictAttributesPresent = true;

        }

        private void InitializeOtherExistingGamePieceAttributes()
        {
            InitializeHealth();
            InitializeMovePoints();
            InitializeConflictAttributes();
            RecalculateExpGate();
        }

        private void InitializeHealth()
        {
            if (_isHealthPresent)
            {
                int maxHealth = _healthBase;
                if (_currentLv > 1)
                    maxHealth += DieRoller.RollManyDice(_healthDieSize, _currentLv - 1);

                _healthReference.SetMaxHealth(maxHealth);
                _healthReference.SetCurrentHealth(maxHealth);
            }
                
        }

        private void InitializeMovePoints()
        {
            if (_isMovementBehaviorPresent)
            {
                int maxMovePoints = _movePointBase;
                if (_currentLv > 1)
                    maxMovePoints += DieRoller.RollManyDice(_movePointDieSize, _currentLv - 1);

                _movementReference.SetMaxMovePoints(maxMovePoints);
            }
        }

        private void InitializeConflictAttributes()
        {
            if (_isConflictAttributesPresent)
            {
                int atk = _atkBase;
                int def = _defBase;
                int damage = _damageBase;

                if (_currentLv > 1)
                {
                    atk += DieRoller.RollManyDice(_atkGrowthDieSize, _currentLv - 1);
                    def += DieRoller.RollManyDice(_defGrowthDieSize, _currentLv - 1);
                    damage += DieRoller.RollManyDice(_damageGrowthDieSize, _currentLv - 1);
                }

                _conflictAttributesRef.SetAtk(atk);
                _conflictAttributesRef.SetDef(def);
                _conflictAttributesRef.SetDamage(damage);
            }
        }

        private void IncrementHealth()
        {
            if (_isHealthPresent)
            {
                int maxHealth = _healthReference.GetMaxHealth();
                maxHealth += DieRoller.RollDie(_healthDieSize);
                _healthReference.SetMaxHealth(maxHealth);
                _healthReference.SetCurrentHealth(maxHealth);
            }
        }

        private void IncrementMovePoints()
        {
            if (_isMovementBehaviorPresent)
            {
                int maxMovePoints = _movementReference.GetMaxMovePoints();
                maxMovePoints += DieRoller.RollDie(_movePointDieSize);
                _movementReference.SetMaxMovePoints(maxMovePoints);
            }
        }

        private void IncrementConflictAttributes()
        {
            if (_isConflictAttributesPresent)
            {
                int atk = _conflictAttributesRef.GetAtk();
                int def = _conflictAttributesRef.GetDef();
                int damage = _conflictAttributesRef.GetDamage();

                atk += DieRoller.RollDie(_atkGrowthDieSize);
                def += DieRoller.RollDie(_defGrowthDieSize);
                damage += DieRoller.RollDie(_damageGrowthDieSize);


                _conflictAttributesRef.SetAtk(atk);
                _conflictAttributesRef.SetDef(def);
                _conflictAttributesRef.SetDamage(damage);
            }
        }

        private void RecalculateExpGate()
        {
            if (_currentLv < _maxLv)
                _nextLvExpGate = (int)(_baseExpGate * (_currentLv - 1) * _expGateGrowthMultiplier);
        }

        private void CheckExpForLvUp()
        {
            if (_currentLv < _maxLv)
            {
                if (_currentExp >= _nextLvExpGate)
                {
                    _currentExp = _currentExp % _nextLvExpGate;
                    int lvUpsDetected = (int)(_currentExp / _nextLvExpGate);

                    for (int i = 0; i < lvUpsDetected; i++)
                        LvUp();
                }
            }
        }




        //Getters, Setters, & Commands

        public GamePiece GetGamePiece()
        {
            return _gamePieceReference;
        }

        public int GetCurrentLv()
        {
            return _currentLv;
        }

        public int GetMaxLv()
        {
            return _maxLv;
        }

        public void SetCurrentLv(int value)
        {
            _currentLv = Mathf.Clamp(value, 1, _maxLv);
            InitializeHealth();
            InitializeMovePoints();
            InitializeConflictAttributes();
            RecalculateExpGate();
        }

        public void LvUp()
        {
            if (_currentLv < _maxLv)
            {
                _currentLv++;
                IncrementHealth();
                IncrementMovePoints();
                IncrementConflictAttributes();
                RecalculateExpGate();
            }
        }

        public int GetCurrentExp()
        {
            return _currentExp;
        }

        public void SetCurrentExp(int value)
        {
            _currentExp = Mathf.Max(0, value);
            CheckExpForLvUp();
        }

        public int GetExpGate()
        {
            return _nextLvExpGate;
        }

        public void GainExp(int value)
        {
            _currentExp += Mathf.Max(0, value);
            CheckExpForLvUp();
        }

        public void ClearExp()
        {
            SetCurrentExp(0);
        }
    }
}

