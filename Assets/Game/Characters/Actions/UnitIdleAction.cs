public class UnitIdleAction : AbstractUnitAction
{
    public UnitIdleAction(AbstractUnit unit)
        : base(unit) { }

    public override bool CheckAction()
    {
        if (unit.UnitAction == this)
            return true;

        if (
            unit.UnitActionType != UnitActionType.Idler
            && unit.UnitActionType != UnitActionType.None
        )
        {
            return false;
        }

        StartAction();
        return true;
    }

    protected override void SetActionType()
    {
        actionType = UnitActionType.Idler;
    }
}
