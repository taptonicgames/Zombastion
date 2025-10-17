using System;
using UnityEngine;

[Serializable]
public struct EquipmentUIData
{
    [field: SerializeField] public string Tittle { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
}