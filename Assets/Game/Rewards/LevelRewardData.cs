using System;
using UnityEngine;

[Serializable]
public class LevelRewardData
{
    [ReadOnlyField] public string Id;
    [field: SerializeField] public RewardData WinData { get; private set; }
    [field: SerializeField] public RewardData LoseData { get; private set; }

    public void Validate()
    {
        WinData.Validate();
        LoseData.Validate();
    }

    public RewardData GetRewardDatasByRoundCompleteType(RoundCompleteType type)
    {
        return type switch
        {
            RoundCompleteType.Win => WinData,
            RoundCompleteType.Fail => LoseData,
            _ => null
        };
    }
}