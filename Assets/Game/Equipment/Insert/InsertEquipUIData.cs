using System;
using UnityEngine;

[Serializable]
public struct InsertEquipUIData
{
    [field: SerializeField] public EquipmentType Type { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
}