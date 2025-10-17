using StarterAssets;

public class UnitPauseAction : AbstractUnitAction
{
    private ThirdPersonController thirdPersonController;

    public UnitPauseAction(AbstractUnit unit)
        : base(unit)
    {
        thirdPersonController = unit.GetComponent<ThirdPersonController>();
    }

    public override bool CheckAction()
    {
        if (unit.UnitActionType == actionType)
            return true;
        return false;
    }

    protected override void SetActionType()
    {
        actionType = UnitActionType.Pause;
    }

    public override void StartAction()
    {
        base.StartAction();

        if (unit is PlayerCharacter)
        {
            thirdPersonController.enabled = false;
            unit.Animator.SetFloat(Constants.SPEED, 0);
            unit.Weapon.StopFire();
        }
    }

    public override void OnFinish()
    {
        if (thirdPersonController)
            thirdPersonController.enabled = true;
    }
}
