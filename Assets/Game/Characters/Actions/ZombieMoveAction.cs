using Zenject;

public class ZombieMoveAction : AbstractUnitAction
{
    [Inject]
    private readonly SceneReferences sceneReferences;
	[Inject]
	private readonly UnitActionPermissionHandler unitActionPermissionHandler;

	public ZombieMoveAction(AbstractUnit unit)
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
        actionType = UnitActionType.Move;
    }

    private bool CheckCondition()
    {
        if (!unitActionPermissionHandler.CheckPermission(actionType, unit.UnitActionType))
            return false;
        return true;
    }


	public override void StartAction()
    {
        base.StartAction();
        unit.Agent.isStopped = false;
        FindTargetAndMove();
        EventBus<GatesFallenEvnt>.Subscribe(OnGatesFallenEvnt);
    }

	private void OnGatesFallenEvnt(GatesFallenEvnt evnt)
	{
        FindTargetAndMove();
	}

	private void FindTargetAndMove()
    {
        var target = sceneReferences.castle.Gates.gameObject.activeSelf
            ? sceneReferences.castle.Gates
            : sceneReferences.zombieTarget;

        unit.Agent.SetDestination(target.position);
    }

    public override void OnFinish()
    {
        base.OnFinish();
        unit.Agent.isStopped = true;
        Dispose();
    }

	public override void Dispose()
	{
		EventBus<GatesFallenEvnt>.Unsubscribe(OnGatesFallenEvnt);
	}
}
