using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class BasePlayerAttackAction : AbstractUnitAction
{
    [Inject]
    protected readonly UnitActionPermissionHandler unitActionPermissionHandler;

    [Inject]
    protected readonly PlayerCharacterModel playerCharacterModel;
    protected AbstractUnit targetUnit;
    protected float angleToEnemy = 360f;

    public BasePlayerAttackAction(AbstractUnit unit)
        : base(unit) { }

    public override bool CheckAction()
    {
        if (unit.UnitActionType != actionType && CheckCondition())
            StartAction();
        if (unit.UnitActionType == actionType)
            return true;
        return false;
    }

    protected override void SetActionType()
    {
        actionType = UnitActionType.Attack;
    }

    protected virtual bool CheckCondition()
    {
        if (!unitActionPermissionHandler.CheckPermission(actionType, unit.UnitActionType))
            return false;

        if (FindEnemy())
            return true;

        return false;
    }

    protected bool FindEnemy()
    {
        int mask = 1 << 8;

        var arr = Physics.OverlapSphere(
            unit.transform.position,
            unit.Weapon.WeaponSOData.AttackDistance,
            mask
        );

        List<AbstractUnit> enemies = new();

        for (int i = 0; i < arr.Length; i++)
        {
            if (
                Vector3.Distance(arr[i].transform.position, unit.transform.position)
                < unit.Weapon.WeaponSOData.MinAttackDistance
            )
            {
                continue;
            }

            var enemy = arr[i].GetComponent<AbstractUnit>();

            if (enemy && enemy.IsEnable)
            {
                enemies.Add(enemy);
            }
        }

        if (enemies.Count > 0)
        {
            enemies = enemies
                .OrderBy(a => Vector3.Distance(unit.transform.position, a.transform.position))
                .ToList();

            targetUnit = enemies.First();
            return true;
        }

        return false;
    }

    public override void StartAction()
    {
        base.StartAction();
        WaitRotatingToTarget().Forget();
    }

    protected virtual async UniTask WaitRotatingToTarget()
    {
        await UniTask.WaitUntil(
            () => angleToEnemy < Constants.ALMOST_ZERO,
            cancellationToken: targetUnit.destroyCancellationToken
        );

        await UniTask.WaitForSeconds(0.2f, cancellationToken: targetUnit.destroyCancellationToken);

        if (unit.Health == 0 || !targetUnit || actionType != unit.UnitActionType)
            return;

        await UniTask.WaitUntil(
            () => unit.Weapon.IsReady,
            cancellationToken: unit.destroyCancellationToken
        );

        if (targetUnit)
        {
            FireWeapon();
        }
    }

    protected virtual void FireWeapon()
    {
        unit.Weapon.Fire(unit, targetUnit);
    }

    public override void Update()
    {
        RotateToTarget();
    }

    protected virtual void RotateToTarget()
    {
        if (targetUnit)
        {
            angleToEnemy = StaticFunctions.ObjectFinishTurning(
                unit.transform,
                targetUnit.transform.position,
                -Constants.UNIT_ROTATION_SPEED,
                Constants.UNIT_ROTATION_SPEED
            );
        }
    }

    protected void CheckTargetUnit()
    {
        if (!unit.Weapon.InFire)
            return;

        if (targetUnit && targetUnit.Health > 0)
        {
            if (
                Vector3.Distance(unit.transform.position, targetUnit.transform.position)
                > unit.Weapon.WeaponSOData.AttackDistance + Constants.ATTACK_DISTANCE_INCREMENT
            )
            {
                unit.SetActionTypeForced(UnitActionType.Idler);
            }
        }
        else
        {
            playerCharacterModel.Experience.Value += (
                (AbstractEnemy)targetUnit
            ).ExperienceForDestroy;

            unit.SetActionTypeForced(UnitActionType.Idler);
        }
    }

    public override void OnFinish()
    {
        unit.Weapon.StopFire();
        angleToEnemy = 360f;
        targetUnit = null;
    }
}
