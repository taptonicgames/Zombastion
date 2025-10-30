using UnityEngine;

public class PlayerHealAbility : AbstractUnitAbility
{
    public PlayerFindingCastleType PlayerFindingCastleType { get; private set; } =
        PlayerFindingCastleType.Left;
    private PlayerCharacter abstractPlayerUnit;
    private float ressurectHealth;

    public PlayerHealAbility(AbstractUnit unit)
        : base(unit)
    {
        EventBus<PlayerFindingCastleEvnt>.Subscribe(OnPlayerFindingCastleEvnt);
        abstractPlayerUnit = (PlayerCharacter)unit;
    }

    private void OnPlayerFindingCastleEvnt(PlayerFindingCastleEvnt evnt)
    {
        PlayerFindingCastleType = evnt.type;
        ressurectHealth = unit.Health;
    }

    protected override void SetAbilityType()
    {
        abilityType = AbilityType.Heal;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (PlayerFindingCastleType == PlayerFindingCastleType.Entered)
        {
            ressurectHealth += abstractPlayerUnit.HealthResurectionOnBase / 50f;
            unit.Health = Mathf.FloorToInt(ressurectHealth);
        }
    }
}
