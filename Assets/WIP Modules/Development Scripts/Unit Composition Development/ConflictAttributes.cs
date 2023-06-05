using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SullysToolkit
{
    public class ConflictAttributes : MonoBehaviour, IConflictAttributes
    {
        //Declarations
        [SerializeField] private int _attack;
        [SerializeField] private int _defence;
        [SerializeField] private int _damage;




        //Monobehaviours





        //Internal Utils




        //Getters, Setters, & Commands
        public int GetAtk()
        {
            return _attack;
        }

        public int GetDamage()
        {
            return _damage;
        }

        public int GetDef()
        {
            return _defence;
        }

        public void SetAtk(int value)
        {
            _attack = Mathf.Max(0, value);
        }

        public void SetDamage(int value)
        {
            _damage = Mathf.Max(0, value);
        }

        public void SetDef(int value)
        {
            _defence = Mathf.Max(0, value);
        }
    }
}

