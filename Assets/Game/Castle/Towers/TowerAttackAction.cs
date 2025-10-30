using Cysharp.Threading.Tasks;
using UnityEngine;

public class TowerAttackAction : BasePlayerAttackAction
{
    private float angleToEnemyVertical = 360f;
    private bool CanRotateYAxe => unit.Weapon.WeaponSOData.WeaponType == WeaponType.Crossbow;

    public TowerAttackAction(AbstractUnit unit)
        : base(unit) { }

    protected override async UniTask WaitRotatingToTarget()
    {
        if (CanRotateYAxe)
            await UniTask.WaitUntil(() => angleToEnemyVertical <= Constants.ALMOST_ZERO);
        
        await base.WaitRotatingToTarget();

		await UniTask.WaitUntil(
				() => unit.Weapon.IsReady,
				cancellationToken: unit.destroyCancellationToken
			);

        unit.SetActionTypeForced(UnitActionType.Idler);
    }

    protected override void RotateToTarget()
    {
        if (!targetUnit)
            return;

        angleToEnemy = StaticFunctions.ObjectFinishTurning(
            unit.Weapon.transform,
            targetUnit.transform.position,
            -Constants.UNIT_ROTATION_SPEED,
            Constants.UNIT_ROTATION_SPEED
        );

        if (CanRotateYAxe)
        {
            angleToEnemyVertical = StaticFunctions.ObjectFinishTurning(
                unit.Weapon.transform,
                targetUnit.transform.position,
                -Constants.UNIT_ROTATION_SPEED,
                Constants.UNIT_ROTATION_SPEED,
                axis: RectTransform.Axis.Vertical
            );
        }
    }
}
