using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentSO", menuName = "ScriptableObjects/EquipmentSO")]
public class EquipmentSO : ScriptableObject
{
    [field: SerializeField] public string Id { get; private set; }
    [field: SerializeField] public EquipmentType Type { get; private set; }
    [field: SerializeField] public EquipmentRarity Rarity { get; private set; }
    [field: SerializeField] public int Damage { get; private set; }
    [field: SerializeField] public int[] EnchanceLevels { get; private set; }
    [field: SerializeField] public EquipmentUIData UIData { get; private set; }

    private void OnValidate()
    {
        if (Id != $"{Rarity}-{Type}")
            Id = $"{Rarity}-{Type}";
    }
}