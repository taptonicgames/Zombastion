using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public class LevelRewardData
{
    [ReadOnlyField] public string Id;
    [field: SerializeField] public RewardData[] WinDatas {  get; private set; }
    [field: SerializeField] public RewardData[] LoseDatas {  get; private set; }
}