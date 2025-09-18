public class EnemyMovementAbility : AbstractUnitAbility
{
	private AbstractEnemy enemy;

	public EnemyMovementAbility(AbstractUnit unit) : base(unit)
	{
		enemy = (AbstractEnemy)unit;
	}

	protected override void SetAbilityType()
	{
		abilityType = AbilityType.Movement;
	}

	public override void Update()
	{
		enemy.Animator.SetFloat(Constants.SPEED, enemy.Agent.desiredVelocity.magnitude);
	}
}
