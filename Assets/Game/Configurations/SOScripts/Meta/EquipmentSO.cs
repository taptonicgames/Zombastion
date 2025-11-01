using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentSO", menuName = "ScriptableObjects/EquipmentSO")]
public class EquipmentSO : ScriptableObject
{
    [field: SerializeField] public string Id { get; private set; }
    [field: SerializeField] public EquipmentType Type { get; private set; }
    [field: SerializeField] public RarityType Rarity { get; private set; }
    [field: SerializeField] public CurrencyType UpgradeCurrency { get; private set; }
    [field: SerializeField] public int InsertAvilableAmount { get; private set; }
    [field: SerializeField] public int Value { get; private set; }
    [field: SerializeField] public float GrowFactor { get; private set; }
    [field: SerializeField] public int[] EnchanceLevels { get; private set; }
    [field: SerializeField] public EquipmentUIData UIData { get; private set; }

    private void OnValidate()
    {
        if (Id != $"{Rarity}{Type}")
            Id = $"{Rarity}{Type}";
    }
}