using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class CurrencyData
{
    [ReadOnlyField] public string Id;
    [field: SerializeField] public CurrencyType Type {  get; private set; }
    [field: SerializeField] public CurrencyUIData UIData {  get; private set; }
}