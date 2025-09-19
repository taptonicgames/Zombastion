public class SimpleEnemy : AbstractEnemy
{
    private void Start()
    {
        unitActionsList.Add(new EnemyMoveAction(this));

        foreach (var item in unitActionsList)
        {
            diContainer.Inject(item);
        }

        SetActionTypeForced(UnitActionType.Move);
    }
}
