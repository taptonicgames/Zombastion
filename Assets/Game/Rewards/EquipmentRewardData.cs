using System;
using UnityEngine;

[Serializable]
public class EquipmentRewardData : AbstractRewardData
{
    [field: SerializeField] public EquipmentType Type { get; private set; }
    [field: SerializeField] public RarityType Rarity { get; private set; }

    public void Validate()
    {
        Id = $"{Type}-{Rarity}";
    }
}