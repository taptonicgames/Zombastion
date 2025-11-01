using System;
using UnityEngine;

[Serializable]
public class CurrencySpritesData : AbstractSpritesData
{
    [field: SerializeField] public CurrencyType Type { get; private set; }

    public CurrencySpritesData(CurrencyType type)
    {
        Type = type;
    }

    public void Validate()
    {
        Id = $"{Type}";
    }
}