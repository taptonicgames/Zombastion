using System;
using UnityEngine;

[Serializable]
public class LevelRewardData
{
    [ReadOnlyField] public string Id;
    [field: SerializeField] public RewardData[] WinDatas {  get; private set; }
    [field: SerializeField] public RewardData[] LoseDatas {  get; private set; }

    public RewardData[] GetRewardDatasByRoundCompleteType(RoundCompleteType type)
    {
        return type switch
        {
            RoundCompleteType.Win => WinDatas,
            RoundCompleteType.Fail => LoseDatas,
            _ => new RewardData[0]
        };
    }
}