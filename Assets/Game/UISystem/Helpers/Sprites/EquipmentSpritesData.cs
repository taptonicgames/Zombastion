using System;
using UnityEngine;

[Serializable]
public class EquipmentSpritesData : AbstractSpritesData
{
    [field: SerializeField] public EquipmentType Type { get; private set; }

    public EquipmentSpritesData(EquipmentType type)
    {
        Type = type;
    }

    public void Validate()
    {
        Id = $"{Type}";
    }
}