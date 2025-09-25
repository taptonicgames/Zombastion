public class SimpleEnemy : AbstractEnemy
{
    private void Start()
    {
        abilitiesPair = new()
        {
            { AbilityType.Movement, new EnemyMovementAbility(this) },
        };

        unitActionsList = new() { new UnitIdleAction(this), new ZombieMoveAction(this) };

        foreach (var item in unitActionsList)
        {
            diContainer.Inject(item);
        }

        SetActionTypeForced(UnitActionType.Move);
    }
}
