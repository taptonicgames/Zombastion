using Cysharp.Threading.Tasks;
using UnityEngine;

public class TowerAttackAction : BasePlayerAttackAction
{
    private float angleToEnemyVertical = 360f;

    public TowerAttackAction(AbstractUnit unit)
        : base(unit) { }

    protected override async UniTask WaitRotatingToTarget()
    {
        await UniTask.WaitUntil(() => angleToEnemyVertical <= Constants.ALMOST_ZERO);
        await base.WaitRotatingToTarget();
    }

    public override void Update()
    {
        base.Update();
        CheckTargetUnit();
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

        angleToEnemyVertical = StaticFunctions.ObjectFinishTurning(
            unit.Weapon.transform,
            targetUnit.transform.position + Vector3.up * 1.5f,
            -Constants.UNIT_ROTATION_SPEED,
            Constants.UNIT_ROTATION_SPEED,
            axis: RectTransform.Axis.Vertical
        );
    }
}
