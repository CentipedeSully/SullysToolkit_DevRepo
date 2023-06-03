using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SullysToolkit
{
    public interface IMoveablePiece
    {
        GamePiece GetGamePiece();

        void MoveToNeighborCell((int, int) xyDirection);
    }

    public interface IInteractablePiece
    {
        GamePiece GetGamePiece();

        void Interact();
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


    public interface ILevelable
    {
        GamePiece GetGamePiece();

        void LevelUp();

        void SetLevel();

        int GetLevel();
    }




}
