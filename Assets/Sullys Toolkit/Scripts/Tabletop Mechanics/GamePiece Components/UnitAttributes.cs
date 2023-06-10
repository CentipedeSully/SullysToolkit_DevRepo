using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SullysToolkit
{
    public class UnitAttributes : MonoBehaviour, IAttributes, IRegenerateable, IExperienceProvider
    {
        //Declarations
        [SerializeField] private int _currentActionPoints = 1;
        [SerializeField] private int _maxActionPoints = 1;
        [SerializeField] private int _atkDie = 20;
        [SerializeField] private int _atkModifier = 0;
        [SerializeField] private int _dmgDie = 3;
        [SerializeField] private int _damageModifier = 0;
        [SerializeField] private int _defence = 10;

        [SerializeField] private bool _useCustomExpValue;
        [SerializeField] private int _expValue = 0;





        //Monobehaviours
       private void OnEnable()
        {
            UpdateExpValue();
        }




        //Internal Utils
        private void UpdateExpValue()
        {
            if (_useCustomExpValue == false)
                _expValue = _defence + _damageModifier + _atkModifier + Mathf.FloorToInt(_dmgDie / 2) + _maxActionPoints;
        }



        //Getters, Setters, & Commands
        public int GetAtkDie()
        {
            return _atkDie;
        }

        public int GetAtkModifier()
        {
            return _atkModifier;
        }

        public int GetCurrentActionPoints()
        {
            return _currentActionPoints;
        }

        public int GetDamageDie()
        {
            return _dmgDie;
        }

        public int GetDamageModifier()
        {
            return _damageModifier;
        }

        public int GetDef()
        {
            return _defence;
        }

        public int GetExpValue()
        {
            return _expValue;
        }

        public int GetMaxActionPoints()
        {
            return _maxActionPoints;
        }

        public void RegenerateAttributes()
        {
            SetCurrentActionPoints(_maxActionPoints);
        }

        public void SetAtkDie(int value)
        {
            _atkDie = Mathf.Max(value, 1);
        }

        public void SetAtkModifier(int value)
        {
            _atkModifier = value;
            UpdateExpValue();
        }

        public void SetCurrentActionPoints(int value)
        {
            _currentActionPoints = Mathf.Clamp(value,0,_maxActionPoints);
        }

        public void SetDamageDie(int value)
        {
            _dmgDie = Mathf.Max(1, value);
            UpdateExpValue();
        }

        public void SetDamageModifier(int value)
        {
            _damageModifier = value;
            UpdateExpValue();
        }

        public void SetDef(int value)
        {
            _defence = Mathf.Max(0, value);
            UpdateExpValue();
        }

        public void SetExpValue(int value)
        {
            _expValue = Mathf.Max(value, 0);
        }

        public void SetMaxActionPoints(int value)
        {
            _maxActionPoints = Mathf.Max(0, value);
            SetCurrentActionPoints(_currentActionPoints);
            UpdateExpValue();
        }
    }
}

