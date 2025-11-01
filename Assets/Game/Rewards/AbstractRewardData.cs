using System;
using UnityEngine;

[Serializable]
public abstract class AbstractRewardData
{
    [field: SerializeField] public string Id { get; protected set; }
    [field: SerializeField] public int Value { get; protected set; }
}