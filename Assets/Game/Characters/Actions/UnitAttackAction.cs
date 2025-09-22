using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class UnitAttackAction : AbstractUnitAction
{
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
        var arr = Physics.OverlapSphere(
            unit.transform.position,
            unit.Weapon.WeaponSOData.AttackDistance
        );

        for (int i = 0; i < arr.Length; i++)
        {
            var unit = arr[i].GetComponent<AbstractUnit>();

            if (unit)
            {
                if (attackAbility.EnemyTypesForUnit.Contains(unit.CharacterType))
                {
                    targetUnit = unit;
                    return true;
                }
            }
        }

        return false;
    }

    public override void StartAction()
    {
        base.StartAction();
        RotateToTarget().Forget();
    }

    private async UniTask RotateToTarget()
    {
        await UniTask.WaitUntil(() => unit.ObjectFinishTurning(targetUnit.transform.position) < 1);
        unit.Weapon.Fire();
    }

    public override void Update()
    {
        unit.ObjectFinishTurning(targetUnit.transform.position);

		if (
            Vector3.Distance(unit.transform.position, targetUnit.transform.position)
            > unit.Weapon.WeaponSOData.AttackDistance
        )
        {
            unit.SetActionTypeForced(UnitActionType.Idler);
        }
    }

	public override void OnFinish()
	{
        unit.Weapon.StopFire();
	}
}
