using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SullysToolkit
{
    public class UnitAttributes : MonoBehaviour, IAttributes
    {
        //Declarations
        [SerializeField] private int _currentActionPoints = 1;
        [SerializeField] private int _maxActionPoints = 1;
        [SerializeField] private int _atkDie = 20;
        [SerializeField] private int _atkModifier = 0;
        [SerializeField] private int _dmgDie = 3;
        [SerializeField] private int _damageModifier = 0;
        [SerializeField] private int _defence = 10;






        //Monobehaviours





        //Internal Utils




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

        public int GetMaxActionPoints()
        {
            return _maxActionPoints;
        }

        public void SetAtkDie(int value)
        {
            _atkDie = Mathf.Max(value, 1);
        }

        public void SetAtkModifier(int value)
        {
            _atkModifier = value;
        }

        public void SetCurrentActionPoints(int value)
        {
            _currentActionPoints = Mathf.Max(0, value);
        }

        public void SetDamageDie(int value)
        {
            _dmgDie = Mathf.Max(1, value);
        }

        public void SetDamageModifier(int value)
        {
            _damageModifier = value;
        }

        public void SetDef(int value)
        {
            _defence = Mathf.Max(0, value);
        }

        public void SetMaxActionPoints(int value)
        {
            _maxActionPoints = Mathf.Max(0, value);
        }
    }
}

