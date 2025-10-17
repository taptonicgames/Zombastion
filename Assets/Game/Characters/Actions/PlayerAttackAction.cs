using StarterAssets;
using UnityEngine;

public class PlayerAttackAction : BasePlayerAttackAction
{
    private ThirdPersonController thirdPersonController;
    private PlayerHealAbility healAbility;

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

        if (healAbility.PlayerFindingCastleType == PlayerFindingCastleType.Left && CheckCondition())
        {
            StartAction();
            return true;
        }

        return false;
    }

    public override void StartAction()
    {
        base.StartAction();
        thirdPersonController.EnablePersonRotation = false;
        unit.Animator.SetBool(Constants.ATTACK, true);
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
        base.Update();

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

        CheckTargetUnit();
    }

    public override void OnFinish()
    {
        base.OnFinish();
        unit.Animator.SetBool(Constants.ATTACK, false);
        thirdPersonController.EnablePersonRotation = true;
        thirdPersonController.EnablePersonAnimation = true;
    }
}
