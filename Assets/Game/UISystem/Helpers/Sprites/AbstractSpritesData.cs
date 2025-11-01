using System;
using UnityEngine;

[Serializable]
public abstract class AbstractSpritesData
{
    [field: SerializeField] public string Id { get; protected set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
}