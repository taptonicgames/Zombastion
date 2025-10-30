using System;
using UnityEngine;

[Serializable]
public class UpgradeData
{
    [field: SerializeField] public string Id { get; private set; }
    [field: SerializeField] public PriceData[] Datas { get; private set; }
}