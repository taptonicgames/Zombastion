using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public struct PriceData
{
    [field: SerializeField] public CurrencyType CurrencyType { get; private set; }
    [field: SerializeField] public int StartPrice { get; private set; }
    [field: SerializeField] public float PriceModifier { get; private set; }
}