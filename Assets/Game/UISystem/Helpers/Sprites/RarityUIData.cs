using System;
using UnityEngine;

[Serializable]
public class RarityUIData
{
    [field: SerializeField] public string id { get; private set; }
    [field: SerializeField] public RarityType Type { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }

    public void Validate()
    {
        id = $"{Type}";
    }
}