using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public struct TowerUIData
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
}