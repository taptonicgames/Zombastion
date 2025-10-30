using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "InsertSO", menuName = "ScriptableObjects/InsertSO")]
public class InsertSO : ScriptableObject
{
    [field: SerializeField] public EquipmentType EquipmentType { get; private set; }
    [field: SerializeField] public EquipmentRarity Rarity { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
}