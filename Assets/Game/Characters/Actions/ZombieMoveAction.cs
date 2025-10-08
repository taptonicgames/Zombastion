using Zenject;

public class ZombieMoveAction : AbstractUnitAction
{
    [Inject] private readonly SceneReferences sceneReferences;

    public ZombieMoveAction(AbstractUnit unit)
        : base(unit) { }

    public override bool CheckAction()
    {
        if (unit.UnitActionType == actionType)
            return true;
        return false;
    }

    protected override void SetActionType()
    {
        actionType = UnitActionType.Move;
    }

	public override void StartAction()
	{
		base.StartAction();
        unit.Agent.isStopped = false;
        unit.Agent.SetDestination(sceneReferences.zombieTarget.position);
	}

	public override void OnFinish()
	{
		base.OnFinish();
        unit.Agent.isStopped = true;
	}
}
