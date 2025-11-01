using System;
using UnityEngine;

[Serializable]
public class CurrencyRewardData : AbstractRewardData
{
    [field: SerializeField] public CurrencyType Type { get; private set; }

    public void Validate()
    {
        Id = $"{Type}";
    }
}