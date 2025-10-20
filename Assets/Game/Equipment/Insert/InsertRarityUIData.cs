using System;
using UnityEngine;

[Serializable]
public struct InsertRarityUIData
{
    [field: SerializeField] public EquipmentRarity Rarity { get; private set; }
    [field: SerializeField] public Sprite BG { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
}