using UnityEngine;

[CreateAssetMenu(fileName = "InsertSO", menuName = "ScriptableObjects/InsertSO")]
public class InsertSO : ScriptableObject
{
    [field: SerializeField] public string Id { get; private set; }
    [field: SerializeField] public EquipmentType EquipmentType { get; private set; }
    [field: SerializeField] public InsertType Type { get; private set; }
    [field: SerializeField] public RarityType Rarity { get; private set; }
    [field: SerializeField] public float PercentageBonus { get; private set; }
    [field: SerializeField] public EquipmentUIData UIData { get; private set; }

    private void OnValidate()
    {
        if (Id != $"{Rarity}{Type}")
            Id = $"{Rarity}{Type}";
    }
}