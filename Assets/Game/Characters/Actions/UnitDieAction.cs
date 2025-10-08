public class UnitDieAction : AbstractUnitAction
{
    private RagdollHandler ragdollHandler;

    public UnitDieAction(AbstractUnit unit)
        : base(unit)
    {
        ragdollHandler = unit.GetComponent<RagdollHandler>();
    }

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
        actionType = UnitActionType.Die;
    }

    public override void StartAction()
    {
        base.StartAction();
        ragdollHandler.EnableRagdoll(true);
    }

    private bool CheckCondition()
    {
        if (unit.Health == 0)
            return true;

        return false;
    }
}
