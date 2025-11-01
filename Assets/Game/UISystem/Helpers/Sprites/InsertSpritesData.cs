using System;
using UnityEngine;

[Serializable]
public class InsertSpritesData : AbstractSpritesData
{
    [field: SerializeField] public InsertType Type { get; private set; }

    public InsertSpritesData(InsertType type)
    {
        Type = type;
    }

    public void Validate()
    {
        Id = $"{Type}";
    }
}