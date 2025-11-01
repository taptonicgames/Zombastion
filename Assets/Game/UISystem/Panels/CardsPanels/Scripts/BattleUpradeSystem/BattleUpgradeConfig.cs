using UnityEngine;

[CreateAssetMenu(fileName = "BattleUpgradeConfig", menuName = "Configs/Upgrades/BattleUpgradeConfig")]
public class BattleUpgradeConfig : ScriptableObject
{
    [field: Header("General")]
	[field: SerializeField] public string Id { get; private set; }
	[field: SerializeField] public WeaponType WeaponType { get; private set; }
	[field: SerializeField] public CharacterType CharacterType { get; private set; }
    [field: SerializeField] public BattleUpgradeType UpgradeType { get; private set; }
	[field: SerializeField] public BattleUpgradeRareType RareType { get; private set; }
	[field: Space(10)]
    [field: Header("Card view option")]
    [field: SerializeField] public Sprite UpgradeIcon { get; private set; }
    [field: SerializeField] public string Tittle { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public string Prefix { get; private set; }
    [field: SerializeField] public string Postfix { get; private set; }

    [field: Space(10)]
    [field: Header("Picked option")]
    [field: SerializeField] public bool IsAccumulating { get; private set; }
    [field: SerializeField] public int MaxPickedCount { get; private set; }

    [field: Space(10)]
    [field: Header("Chance show")]
    [field: SerializeField] public float ChanceShowingPercentage { get; private set; }

    [field: Space(10)]
    [field: Header("Value")]
    [field: SerializeField] public float Value { get; private set; }

    private void OnValidate()
    {
        if (Id != $"{CharacterType}-{WeaponType}")
            Id = $"{CharacterType}-{WeaponType}";
    }

    public virtual T GetParameterType<T>()
    {
        return default;
    }
}