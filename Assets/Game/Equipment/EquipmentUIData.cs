using System;
using UnityEngine;

[Serializable]
public struct EquipmentUIData
{
    [field: SerializeField] public string Tittle { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public Sprite EquipmentIcon { get; private set; }
    [field: SerializeField] public Sprite BG { get; private set; }
}