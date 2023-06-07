using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace SullysToolkit
{
    public class Health : MonoBehaviour, IHealthManager, IHealablePiece, IDamageablePiece
    {

        //Declarations
        [Header("Health Settings")]
        [SerializeField] private int _currentHealth = 1;
        [SerializeField] [Min(1)] private int _maxHealth = 1;

        [Header("References")]
        [SerializeField] private GamePiece _gamePieceReference;

        //Events
        public delegate void HealthEvent(int value);
        public event HealthEvent OnHealed;
        public event HealthEvent OnDamaged;



        //Monobehaviours
        private void Awake()
        {
            InitializeSettings();
        }


        //Internal Utils
        private void InitializeSettings()
        {
            _gamePieceReference = GetComponent<GamePiece>();
            _currentHealth = _maxHealth;
        }


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

        public GamePiece GetGamePiece()
        {
            return _gamePieceReference;
        }

        public void ReceiveHeals(int value)
        {
            int healValue = Mathf.Max(0, value);
            SetCurrentHealth(_currentHealth + healValue);

            OnHealed?.Invoke(healValue);
        }

        public void RecieveDamage(int value)
        {
            int damageValue = Mathf.Max(0, value);
            SetCurrentHealth(_currentHealth - damageValue);

            OnDamaged?.Invoke(damageValue);
        }

        public void KillThisInstance()
        {
            //...
        }
    }
}


