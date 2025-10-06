using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class ZombieAttackAction : AbstractUnitAction
{
    [Inject]
    private readonly UnitActionPermissionHandler unitActionPermissionHandler;

    [Inject]
    private readonly SceneReferences sceneReferences;
    private AbstractUnit targetUnit;
    private Transform targetTr;
    private float angleToTarget = 360f;

    public ZombieAttackAction(AbstractUnit unit)
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

    private bool CheckCondition()
    {
        if (!unitActionPermissionHandler.CheckPermission(actionType, unit.UnitActionType))
            return false;

        if (FindTargetTr() || FindPlayer())
        {
            StartAction();
            return true;
        }

        return false;
    }

    private bool FindTargetTr()
    {
        int mask = 1 << 9;

        var arr = Physics.OverlapSphere(
            unit.transform.position,
            unit.Weapon.WeaponSOData.AttackDistance,
            mask
        );

        if (arr.Length > 0 && sceneReferences.castle.Health > 0)
        {
            targetTr = arr[0].transform;
            return true;
        }

        return false;
    }

    private bool FindPlayer()
    {
        int mask = 1 << 3;

        var arr = Physics.OverlapSphere(
            unit.transform.position,
            unit.Weapon.WeaponSOData.AttackDistance,
            mask
        );

        List<AbstractUnit> playerCharacters = new();

        for (int i = 0; i < arr.Length; i++)
        {
            var character = arr[i].GetComponent<AbstractUnit>();

            if (character)
            {
                if (character.Health == 0 || !character.IsEnable)
                    continue;

                playerCharacters.Add(character);
            }
        }

        if (playerCharacters.Count > 0)
        {
            playerCharacters.OrderBy(a =>
                Vector3.Distance(unit.transform.position, a.transform.position)
            );
            targetUnit = playerCharacters.First();
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
        if (targetUnit)
        {
            await UniTask.WaitUntil(
                () => angleToTarget < Constants.ALMOST_ZERO,
                cancellationToken: targetUnit.destroyCancellationToken
            );
        }

        if (targetTr)
        {
            await UniTask.WaitUntil(
                () => angleToTarget < Constants.ALMOST_ZERO,
                cancellationToken: unit.destroyCancellationToken
            );
        }

        unit.Weapon.Fire(unit, targetUnit);
    }

    public override void Update()
    {
        if (targetUnit)
        {
            angleToTarget = StaticFunctions.ObjectFinishTurning(
                unit.transform,
                targetUnit.transform.position,
                -Constants.UNIT_ROTATION_SPEED,
                Constants.UNIT_ROTATION_SPEED
            );
        }

        if (targetTr)
        {
            angleToTarget = StaticFunctions.ObjectFinishTurning(
                unit.transform,
                targetTr.position,
                -Constants.UNIT_ROTATION_SPEED,
                Constants.UNIT_ROTATION_SPEED
            );
        }

        if (!unit.Weapon.InFire)
            return;

        if (targetUnit && targetUnit.Health > 0)
        {
            if (
                Vector3.Distance(unit.transform.position, targetUnit.transform.position)
                > unit.Weapon.WeaponSOData.AttackDistance + Constants.ATTACK_DISTANCE_INCREMENT
            )
            {
                unit.SetActionTypeForced(UnitActionType.Move);
            }
        }
        else if (!targetTr)
        {
            unit.SetActionTypeForced(UnitActionType.Move);
        }
        else if (targetTr)
        {
            if (sceneReferences.castle.Health == 0)
                unit.SetActionTypeForced(UnitActionType.Move);
        }
    }

    public override void OnFinish()
    {
        unit.Weapon.StopFire();
        unit.Animator.SetBool(Constants.ATTACK, false);
        angleToTarget = 360f;
        targetUnit = null;
    }
}
