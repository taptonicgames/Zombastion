public class EnemyMovementAbility : AbstractUnitAbility
{
    private AbstractEnemy enemy;
    private const float MAGNITUDE_ANIMATION_INCREMENT = 1.7f;

    public EnemyMovementAbility(AbstractUnit unit)
        : base(unit)
    {
        enemy = (AbstractEnemy)unit;
    }

    protected override void SetAbilityType()
    {
        abilityType = AbilityType.Movement;
    }

    public override void Update()
    {
        enemy.Animator.SetFloat(
            Constants.SPEED,
            enemy.Agent.desiredVelocity.magnitude * MAGNITUDE_ANIMATION_INCREMENT
        );
    }
}
