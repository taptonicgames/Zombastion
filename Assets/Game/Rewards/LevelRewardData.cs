using System;
using UnityEngine;

[Serializable]
public class LevelRewardData
{
    [ReadOnlyField] public string Id;
    [field: SerializeField] public RewardData[] WinDatas {  get; private set; }
    [field: SerializeField] public RewardData[] LoseDatas {  get; private set; }

    public void Validate()
    {
        for (int i = 0; i < WinDatas.Length; i++)
            WinDatas[i].InitId();
        for (int i = 0;i < LoseDatas.Length;i++)
            LoseDatas[i].InitId();
    }

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