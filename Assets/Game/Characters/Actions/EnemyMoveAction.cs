using Zenject;

public class EnemyMoveAction : AbstractUnitAction
{
    [Inject] private readonly SceneReferences sceneReferences;

    public EnemyMoveAction(AbstractUnit unit)
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
        unit.Agent.SetDestination(sceneReferences.zombieTarget.position);
	}
}
