using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class UnitAttackAction : AbstractUnitAction
{
	private const float ROTATION_SPEED = 2.5f;
	private const float ATTACK_DISTANCE_INCREMENT = 3f;
	[Inject]
    UnitActionPermissionHandler unitActionPermissionHandler;
    private AbstractUnit targetUnit;
    private UnitAttackAbility attackAbility;

    public UnitAttackAction(AbstractUnit unit)
        : base(unit)
    {
        attackAbility = unit.GetUnitAbility<UnitAttackAbility>(AbilityType.Attack);
    }

    public override bool CheckAction()
    {
        if (unit.UnitActionType == actionType)
            return true;

        if (unitActionPermissionHandler.CheckPermission(actionType, unit.UnitActionType))
        {
            if (FindEnemy())
            {
                StartAction();
                return true;
            }
        }

        return false;
    }

    protected override void SetActionType()
    {
        actionType = UnitActionType.Attack;
    }

    private bool FindEnemy()
    {
        int mask = ~(1 << 3);

        var arr = Physics.OverlapSphere(
            unit.transform.position,
            unit.Weapon.WeaponSOData.AttackDistance,
            mask
        );

        List<AbstractUnit> enemies = new();

        for (int i = 0; i < arr.Length; i++)
        {
            var enemy = arr[i].GetComponent<AbstractUnit>();

            if (enemy)
            {
                enemies.Add(enemy);
            }
        }

        if (enemies.Count > 0)
        {
            enemies.OrderBy(a => Vector3.Distance(unit.transform.position, a.transform.position));
            targetUnit = enemies.First();
            return true;
        }

        return false;
    }

    public override void StartAction()
    {
        base.StartAction();
        unit.Animator.SetBool(Constants.ATTACK, true);
        RotateToTarget().Forget();
    }

    private async UniTask RotateToTarget()
    {
        await UniTask.WaitUntil(
            () =>
                StaticFunctions.ObjectFinishTurning(unit.transform, targetUnit.transform.position)
                < Constants.ALMOST_ZERO,
            cancellationToken: targetUnit.destroyCancellationToken
        );

        unit.Weapon.Fire(targetUnit);
    }

    public override void Update()
    {
        if (!unit.Weapon.InFire)
            return;

        if (targetUnit && targetUnit.Health > 0)
        {
            StaticFunctions.ObjectFinishTurning(
                unit.transform,
                targetUnit.transform.position,
                -ROTATION_SPEED,
				ROTATION_SPEED
			);

            if (
                Vector3.Distance(unit.transform.position, targetUnit.transform.position)
                > unit.Weapon.WeaponSOData.AttackDistance + ATTACK_DISTANCE_INCREMENT
			)
            {
                unit.SetActionTypeForced(UnitActionType.Idler);
            }
        }
        else
        {
            unit.SetActionTypeForced(UnitActionType.Idler);
        }
    }

    public override void OnFinish()
    {
        unit.Weapon.StopFire();
        unit.Animator.SetBool(Constants.ATTACK, false);
    }
}
