using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SullysToolkit
{
    public interface IMoveablePiece
    {
        GamePiece GetGamePiece();

        void MoveToNeighborCell((int, int) xyDirection);

        int GetCurrentMovePoints();

        void SetCurrentMovePoints(int value);

        int GetMaxMovePoints();

        void SetMaxMovePoints(int value);
    }

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

    public interface IConflictAttributes
    {
        int GetAtk();

        void SetAtk(int value);

        int GetDef();

        void SetDef(int value);

        int GetDamage();

        void SetDamage(int value);
    }


}
