using Zenject;

public class UnitDieAction : AbstractUnitAction
{
	[Inject] private readonly UnitActionPermissionHandler unitActionPermissionHandler;

	public UnitDieAction(AbstractUnit unit) : base(unit)
	{
	}

	public override bool CheckAction()
	{
		if (unit.UnitActionType != actionType && CheckCondition()) StartAction();
		if (unit.UnitActionType == actionType) return true;
		return false;
	}

	protected override void SetActionType()
	{
		actionType = UnitActionType.Die;
	}

	private bool CheckCondition()
	{
		if (!unitActionPermissionHandler.CheckPermission(actionType, unit.UnitActionType)) return false;
		return false;
	}
}
