using System.Collections.Generic;

public class UnitAttackAbility : AbstractUnitAbility
{
    public List<CharacterType> EnemyTypesForUnit { get; }

    public UnitAttackAbility(AbstractUnit unit, List<CharacterType> enemyTypes)
        : base(unit)
    {
        EnemyTypesForUnit = enemyTypes;
    }

    protected override void SetAbilityType()
    {
        abilityType = AbilityType.Attack;
    }
}
