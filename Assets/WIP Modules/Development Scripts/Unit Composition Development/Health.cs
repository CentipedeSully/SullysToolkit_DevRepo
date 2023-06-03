using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace SullysToolkit
{
    public class Health : MonoBehaviour, IHealthManager
    {

        //Declarations
        [Header("Health Settings")]
        [SerializeField] private int _currentHealth = 1;
        [SerializeField] [Min(1)] private int _maxHealth = 1;



        //Monobehaviours



        //Internal Utils



        //Getters, Setters, & Commands
        public int GetCurrentHealth()
        {
            return _currentHealth;
        }

        public void SetCurrentHealth(int value)
        {
            _currentHealth = Mathf.Clamp(value, 0, _maxHealth);
        }

        public int GetMaxHealth()
        {
            return _maxHealth;
        }

        public void SetMaxHealth(int value)
        {
            _maxHealth = Mathf.Max(1, value);

            //Reflect the new maxiumum
            SetCurrentHealth(_currentHealth);
        }





    }
}


