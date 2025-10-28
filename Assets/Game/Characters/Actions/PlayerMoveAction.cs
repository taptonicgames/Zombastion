using StarterAssets;
using UnityEngine;
using Zenject;

public class PlayerMoveAction : AbstractUnitAction
{
    [Inject]
    UnitActionPermissionHandler unitActionPermissionHandler;
    private ThirdPersonController thirdPersonController;

    public PlayerMoveAction(AbstractUnit unit)
        : base(unit)
    {
        thirdPersonController = unit.GetComponent<ThirdPersonController>();
    }

    public override bool CheckAction()
    {
        if (unit.UnitActionType == actionType)
            return true;

        if (unitActionPermissionHandler.CheckPermission(actionType, unit.UnitActionType))
        {
            if (CheckMoving())
            {
                StartAction();
                return true;
            }

            if (unit.UnitActionType != actionType)
                thirdPersonController.StopingAnimation();
        }

        return false;
    }

    protected override void SetActionType()
    {
        actionType = UnitActionType.Move;
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

        if (!CheckMoving())
            unit.SetActionTypeForced(UnitActionType.Idler);
    }
}
