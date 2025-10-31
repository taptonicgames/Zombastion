using System;
using UnityEngine;

[Serializable]
public struct RewardData
{
    [field: SerializeField] public string Id { get; private set; }
    [field: SerializeField] public CurrencyType CurrencyType { get; private set; }
    [field: SerializeField] public int Value { get; private set; }

    public void InitId()
    {
        Id = $"{CurrencyType}";
    }
}