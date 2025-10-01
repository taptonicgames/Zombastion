public class PlayerHealAbility : AbstractUnitAbility
{
	private PlayerFindingCastleType playerFindingCastleType;

	public PlayerHealAbility(AbstractUnit unit) : base(unit)
	{
		EventBus<PlayerFindingCastleEvnt>.Subscribe(OnPlayerFindingCastleEvnt);
	}

	private void OnPlayerFindingCastleEvnt(PlayerFindingCastleEvnt evnt)
	{
		playerFindingCastleType = evnt.type;
	}

	protected override void SetAbilityType()
	{
		abilityType = AbilityType.Heal;
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();

	}
}
