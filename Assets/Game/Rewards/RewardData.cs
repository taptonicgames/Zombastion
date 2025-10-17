using System;
using UnityEngine;

[Serializable]
public struct RewardData
{
    [field: SerializeField] public CurrencyType CurrencyType { get; private set; }
    [field: SerializeField] public int Value { get; private set; }
}