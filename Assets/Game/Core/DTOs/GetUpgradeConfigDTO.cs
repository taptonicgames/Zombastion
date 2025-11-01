public struct GetUpgradeConfigDTO
{
    public readonly CharacterType characterType;
    public readonly WeaponType weaponType;
    public readonly BattleUpgradeType upgradeType;
    public readonly ParameterUpgradeType parameterUpgradeType;
    public readonly BattleUpgradeRareType rareType;

    public GetUpgradeConfigDTO(
        CharacterType characterType = CharacterType.Player,
        WeaponType weaponType = WeaponType.None,
        BattleUpgradeType upgradeType = BattleUpgradeType.None,
        ParameterUpgradeType parameterUpgradeType = ParameterUpgradeType.None,
        BattleUpgradeRareType rareType = BattleUpgradeRareType.None
    )
    {
        this.characterType = characterType;
        this.weaponType = weaponType;
        this.upgradeType = upgradeType;
        this.parameterUpgradeType = parameterUpgradeType;
        this.rareType = rareType;
    }
}
