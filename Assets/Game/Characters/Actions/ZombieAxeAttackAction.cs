using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class ZombieAxeAttackAction : AbstractUnitAction
{
    [Inject]
    private readonly UnitActionPermissionHandler unitActionPermissionHandler;

    [Inject]
    private readonly SceneReferences sceneReferences;

    [Inject]
    private readonly CoroutineManager coroutineManager;
    private AbstractUnit targetUnit;
    private Transform targetTr;
    private Coroutine coroutine,
        cmCoroutine;
    private const float AXE_ATTACK_DISTANCE = 2f;
    private bool canSwitchToMove = true;

    public ZombieAxeAttackAction(AbstractUnit unit)
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
        unit.Agent.isStopped = false;
        coroutine = unit.StartCoroutine(WaitTargetReached());
    }

    private IEnumerator WaitTargetReached()
    {
        if (targetUnit)
        {
            yield return new WaitUntil(() =>
                Vector3.Distance(unit.transform.position, targetUnit.transform.position)
                < AXE_ATTACK_DISTANCE
            );
        }

        if (targetTr)
        {
            yield return new WaitUntil(() =>
                Vector3.Distance(unit.transform.position, targetTr.position) < AXE_ATTACK_DISTANCE
            );
        }

        unit.Agent.isStopped = true;
        canSwitchToMove = false;
        unit.Animator.SetBool(Constants.ATTACK, true);
    }

    public override void Update()
    {
        if (targetUnit)
        {
            unit.Agent.SetDestination(targetUnit.transform.position);
        }

        if (targetTr)
        {
            unit.Agent.SetDestination(targetTr.position);
        }

        if (targetUnit && targetUnit.Health > 0)
        {
            if (
                Vector3.Distance(unit.transform.position, targetUnit.transform.position)
                    > AXE_ATTACK_DISTANCE
                && unit.Animator.GetBool(Constants.ATTACK)
            )
            {
                unit.Animator.SetBool(Constants.ATTACK, false);

                Action action = () =>
                {
                    unit.Agent.isStopped = false;
                    unit.StopCoroutine(coroutine);
                    coroutine = unit.StartCoroutine(WaitTargetReached());
                };

                cmCoroutine = coroutineManager.InvokeWaitUntil(action, () => canSwitchToMove);
            }

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

    public override void SetAnimationPhase(int value)
    {
        base.SetAnimationPhase(value);
        var axe = (HandWeapon)unit.Weapon;

        if (value == 0)
        {
            if (targetUnit)
            {
                if (
                    Vector3.Distance(unit.transform.position, targetUnit.transform.position)
                    <= AXE_ATTACK_DISTANCE
                )
                    unit.Weapon.Fire(unit, targetUnit);
            }
            else if (targetTr)
            {
                unit.Weapon.Fire(unit, targetTr);
            }
        }
        else
        {
            canSwitchToMove = true;
        }
    }

    private void StopCMCoroutine()
    {
        if (coroutineManager && cmCoroutine != null)
            coroutineManager.StopCoroutine(cmCoroutine);
    }

    public override void OnFinish()
    {
        unit.Animator.SetBool(Constants.ATTACK, false);
        targetUnit = null;
        unit.StopCoroutine(coroutine);
        StopCMCoroutine();
    }

    public override void Dispose()
    {
        base.Dispose();
        StopCMCoroutine();
    }
}
