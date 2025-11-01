using System;
using UnityEngine;

[Serializable]
public class InsertRewardData : AbstractRewardData
{
    [field: SerializeField] public InsertType Type { get; private set; }
    [field: SerializeField] public RarityType Rarity { get; private set; }

    public void Validate()
    {
        Id = $"{Type}-{Rarity}";
    }
}