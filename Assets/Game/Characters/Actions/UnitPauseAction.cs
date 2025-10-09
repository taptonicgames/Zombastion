public class UnitPauseAction : AbstractUnitAction
{
    public UnitPauseAction(AbstractUnit unit)
        : base(unit) { }

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
}
