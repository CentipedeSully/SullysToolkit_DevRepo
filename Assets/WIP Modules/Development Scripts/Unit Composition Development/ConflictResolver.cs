using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SullysToolkit
{
    public static class ConflictResolver
    {
        //Declarations
        private static IConflictLogger _conflictLogger;
        private static int _unusedFieldCode = -999;
        private static int _lastAttackerAtkRoll;
        private static int _lastAttackerDef;
        private static int _lastAttackerDmgRoll;
        private static int _lastDefenderAtkRoll;
        private static int _lastDefenderDef;
        private static int _lastDefenderDmgRoll;


        //Internal Utils
        private static void DeductApCost(IAttributes unit)
        {
            if (unit != null)
            {
                if (unit.GetCurrentActionPoints() > 0)
                    unit.SetCurrentActionPoints(unit.GetCurrentActionPoints() - 1);
            }
            else
                STKDebugLogger.LogWarning("Attempted Action Point Deduction from null unit attribute");
        }

        private static int CalculateAttackRoll(IAttributes unit)
        {
            if (unit != null)
                return DieRoller.RollDieWithModifier(unit.GetAtkDie(), unit.GetAtkModifier());
            else
            {
                STKDebugLogger.LogWarning($"Null unit attribute tried to make an atkRoll. Returning {_unusedFieldCode}");
                return _unusedFieldCode;
            }
        }

        private static int CalculateDamageRoll(IAttributes unit)
        {
            if (unit != null)
                return DieRoller.RollDieWithModifier(unit.GetDamageDie(), unit.GetDamageModifier());
            else
            {
                STKDebugLogger.LogWarning($"Null unit attribute tried to roll for Damage. Returning {_unusedFieldCode}");
                return -_unusedFieldCode;
            }
        }

        private static int GetDefence(IAttributes unit)
        {
            if (unit != null)
                return unit.GetDef();
            else
            {
                STKDebugLogger.LogWarning($"Attempted to get Defence for null unit Attribute. Returning {_unusedFieldCode}");
                return -_unusedFieldCode;
            }
        }

        private static void DamageUnit(IDamageablePiece unit, int damage)
        {
            if (unit != null)
                unit.RecieveDamage(damage);
            else
                STKDebugLogger.LogWarning("Attempted to damage a null Damageable unit");
        }

        private static void LogConflict()
        {
            _conflictLogger.LogConflict(_lastAttackerAtkRoll, _lastAttackerDmgRoll, _lastAttackerDef, _lastDefenderAtkRoll, _lastDefenderDmgRoll, _lastDefenderDef);
        }


        //Commands
        public static void ResolveOneSidedConflict(GamePiece attackerGamePiece, GamePiece defenderGamePiece)
        {
            //pay AP cost
            DeductApCost(attackerGamePiece.GetComponent<IAttributes>());

            //Calculate current Conflict stats
            _lastAttackerAtkRoll = CalculateAttackRoll(attackerGamePiece.GetComponent<IAttributes>());
            _lastAttackerDef = GetDefence(attackerGamePiece.GetComponent<IAttributes>());
            _lastAttackerDmgRoll = CalculateDamageRoll(attackerGamePiece.GetComponent<IAttributes>());

            _lastDefenderAtkRoll = _unusedFieldCode;
            _lastDefenderDef = GetDefence(defenderGamePiece.GetComponent<IAttributes>());
            _lastDefenderDmgRoll = _unusedFieldCode;

            LogConflict();

            if (_lastAttackerAtkRoll >= _lastDefenderDef)
                DamageUnit(defenderGamePiece.GetComponent<IDamageablePiece>(), _lastAttackerDmgRoll);
        }

        public static void ResolveTwoSidedConflict(GamePiece attackerGamePiece, GamePiece defenderGamePiece)
        {
            //pay AP cost
            DeductApCost(attackerGamePiece.GetComponent<IAttributes>());
            DeductApCost(defenderGamePiece.GetComponent<IAttributes>());

            //Calculate current Conflict stats
            _lastAttackerAtkRoll = CalculateAttackRoll(attackerGamePiece.GetComponent<IAttributes>());
            _lastAttackerDef = GetDefence(attackerGamePiece.GetComponent<IAttributes>());
            _lastAttackerDmgRoll = CalculateDamageRoll(attackerGamePiece.GetComponent<IAttributes>());

            _lastDefenderAtkRoll = CalculateAttackRoll(defenderGamePiece.GetComponent<IAttributes>());
            _lastDefenderDef = GetDefence(defenderGamePiece.GetComponent<IAttributes>());
            _lastDefenderDmgRoll = CalculateDamageRoll(defenderGamePiece.GetComponent<IAttributes>());

            LogConflict();

            if (_lastAttackerAtkRoll >= _lastDefenderDef)
                DamageUnit(defenderGamePiece.GetComponent<IDamageablePiece>(), _lastAttackerDmgRoll);
            
            if (_lastDefenderAtkRoll >= _lastAttackerDef)
                DamageUnit(attackerGamePiece.GetComponent<IDamageablePiece>(), _lastDefenderDmgRoll);
        }

        public static IConflictLogger GetConflictLogger()
        {
            return _conflictLogger;
        }

        public static void SetConflictLogger(IConflictLogger newConflictLogger)
        {
            if (newConflictLogger != null)
                _conflictLogger = newConflictLogger;
        }
    }
}
