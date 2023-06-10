using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SullysToolkit
{
    public enum GamePieceType
    {
        Terrain,
        PointOfInterest,
        Unit
    }


    public interface IMoveablePiece
    {
        GamePiece GetGamePiece();

        void MoveToNeighborCell((int, int) xyDirection);

        int GetCurrentMovePoints();

        void SetCurrentMovePoints(int value);

        int GetMaxMovePoints();

        void SetMaxMovePoints(int value);
    }

    public interface IDamageablePiece
    {
        GamePiece GetGamePiece();

        void RecieveDamage(int value);
    }

    public interface IHealablePiece
    {
        GamePiece GetGamePiece();

        void ReceiveHeals(int value);
    }

    public interface IHealthManager
    {
        int GetCurrentHealth();

        void SetCurrentHealth(int value);

        int GetMaxHealth();

        void SetMaxHealth(int value);

        void KillThisInstance();
    }

    public interface ILevelablePiece
    {
        GamePiece GetGamePiece();

        int GetCurrentLv();

        int GetMaxLv();

        void SetCurrentLv(int value);

        void LvUp();

        int GetCurrentExp();

        void SetCurrentExp(int value);

        int GetExpGate();

        void GainExp(int value);

        void ClearExp();
    }

    public interface IAttributes
    {
        int GetAtkDie();

        void SetAtkDie(int value);

        int GetAtkModifier();

        void SetAtkModifier(int value);

        int GetDef();

        void SetDef(int value);

        int GetDamageModifier();

        void SetDamageModifier(int value);

        int GetDamageDie();

        void SetDamageDie(int value);

        int GetCurrentActionPoints();

        void SetCurrentActionPoints(int value);

        int GetMaxActionPoints();

        void SetMaxActionPoints(int value);
    }

    public interface IConflictLogger
    {
        void LogConflict(int attackerAtkScore, int attackerDmgScore, int attackerDef, int defenderAtkScore, int defenderDmgScore, int defenderDef);
    }

    public interface IRegenerateable
    {
        void RegenerateAttributes();
    }

    public interface IIdentityDefinition
    {
        GamePiece GetGamePiece();

        GamePieceType GetType();

        string GetName();

        void SetName(string name);

        string GetFaction();

        void SetFaction(string faction);

        bool IsHostile();

        void SetHostility(bool value);

    }

    public interface IExperienceProvider
    {
        int GetExpValue();

        void SetExpValue(int value);
    }

    public interface IGamePieceController
    {
        GamePiece GetGamePiece();

        void MoveInDirection((int,int) xyDirection);

        void InteractWithCellInDirection((int, int) xyDirection);

        void ToggleShowData(bool value);

        bool IsShowDataToggled();
        
    }



    //Currently Work in Progress unused
    public interface IInteractablePiece
    {
        GamePiece GetGamePiece();

        void TriggerEventOnInteraction();
    }

    public interface IInterationPerformerPiece
    {
        GamePiece GetGamePiece();

        void InteractWithPointOfInterest(IInteractablePiece gamePiece);
    }
}
