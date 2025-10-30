using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class ChestRewardData
{
    [ReadOnlyField] public string Id;
    [field: SerializeField] public RewardData[] CommonChestDatas { get; private set; }
    [field: SerializeField] public RewardData[] RareChestDatas { get; private set; }
    [field: SerializeField] public RewardData[] LegendChestDatas { get; private set; }
}