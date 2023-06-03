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

        void TriggerEventOnInteraction();
    }

    public interface IInterationPerformer
    {
        GamePiece GetGamePiece();

        void InteractWithInteractable(IInteractablePiece gamePiece);
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

        int GetNextLvlThreshold();

        int GetMaxLevel();

        void ForceLvlUp();

        int GetCurrentLevel();

        void SetCurrentLevel(int value);

        AnimationCurve GetExpGrowthCurve();

        void SetExpGrowthCurve(AnimationCurve growthCurve);

        AnimationCurve GetHealthGrowthCurve();

        void SetHealthGrowthCurve(AnimationCurve growthCurve);

        AnimationCurve GetAtkGrowthCurve();

        void SetAtkGrowthCurve(AnimationCurve atkCurve);

        AnimationCurve GetDefGrowthCurve();

        void SetDefGrowthCurve();

        AnimationCurve GetActionPointsGrowthCurve();

        void SetActionPointsGrowthCurve(AnimationCurve apCurve);

        AnimationCurve GetMovePointGrowthCurve();

        void SetMovePointGrowthCurve(AnimationCurve mpCurve);

        int GetCurrentExp();

        void SetCurrentExp(int value);

        void GainExp(int value);

        
    }




}
