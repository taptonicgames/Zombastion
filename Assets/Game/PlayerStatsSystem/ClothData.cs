using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public class ClothData
{
    [field: SerializeField] public string Id { get; private set; }
    [field: SerializeField] public ClothType Type { get; private set; }
    [field: SerializeField] public ClothUIData ClothUIData { get; private set; }

    public void InitId()
    {
        Id = $"{Type}";
    }
 }

[Serializable]
public struct ClothUIData
{
    [field: SerializeField] public string Tittle { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
}