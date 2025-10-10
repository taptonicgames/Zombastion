using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using StarterAssets;
using UnityEngine;
using Zenject;

public class PlayerAttackAction : AbstractUnitAction
{
    [Inject]
    private readonly UnitActionPermissionHandler unitActionPermissionHandler;

    [Inject]
    private readonly PlayerCharacterModel playerCharacterModel;
    private AbstractUnit targetUnit;
    private ThirdPersonController thirdPersonController;
    private PlayerHealAbility healAbility;
    private float angleToEnemy = 360f;

    public PlayerAttackAction(AbstractUnit unit)
        : base(unit)
    {
        thirdPersonController = unit.GetComponent<ThirdPersonController>();
        healAbility = unit.GetUnitAbility<PlayerHealAbility>(AbilityType.Heal);
    }

    public override bool CheckAction()
    {
        if (unit.UnitActionType == actionType)
        {
            if (healAbility.PlayerFindingCastleType == PlayerFindingCastleType.Left)
                return true;
            else
            {
                unit.SetActionTypeForced(UnitActionType.Idler);
                return false;
            }
        }

        if (
            healAbility.PlayerFindingCastleType == PlayerFindingCastleType.Left
            && unitActionPermissionHandler.CheckPermission(actionType, unit.UnitActionType)
        )
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

            if (enemy && enemy.IsEnable)
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
        thirdPersonController.EnablePersonRotation = false;
        unit.Animator.SetBool(Constants.ATTACK, true);
        RotateToTarget().Forget();
    }

    private async UniTask RotateToTarget()
    {
        await UniTask.WaitUntil(
            () => angleToEnemy < Constants.ALMOST_ZERO,
            cancellationToken: targetUnit.destroyCancellationToken
        );

        await UniTask.WaitForSeconds(0.2f);

        if (unit.Health == 0)
            return;

        unit.Weapon.Fire(unit, targetUnit);
    }

    private bool CheckMoving()
    {
        return thirdPersonController.Input.move != Vector2.zero;
    }

    public override void Update()
    {
        thirdPersonController.JumpAndGravity();
        thirdPersonController.GroundedCheck();
        thirdPersonController.Move();

        if (targetUnit)
        {
            angleToEnemy = StaticFunctions.ObjectFinishTurning(
                unit.transform,
                targetUnit.transform.position,
                -Constants.UNIT_ROTATION_SPEED,
                Constants.UNIT_ROTATION_SPEED
            );
        }

        if (targetUnit && !CheckMoving())
        {
            thirdPersonController.EnablePersonAnimation = false;

            if (Mathf.Abs(angleToEnemy) > 1f)
                unit.Animator.SetFloat(Constants.SPEED, 3);
            else
                unit.Animator.SetFloat(Constants.SPEED, 0);
        }
        else
        {
            thirdPersonController.EnablePersonAnimation = true;
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
                unit.SetActionTypeForced(UnitActionType.Idler);
            }
        }
        else
        {
            playerCharacterModel.Experience.Value += ((AbstractEnemy)targetUnit).ExperienceForDestroy;
            unit.SetActionTypeForced(UnitActionType.Idler);
        }
    }

    public override void OnFinish()
    {
        unit.Weapon.StopFire();
        unit.Animator.SetBool(Constants.ATTACK, false);
		thirdPersonController.EnablePersonRotation = true;
        thirdPersonController.EnablePersonAnimation = true;
        angleToEnemy = 360f;
        targetUnit = null;
    }
}
