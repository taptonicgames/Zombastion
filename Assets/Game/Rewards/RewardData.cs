using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RewardData
{
    [field: SerializeField] public CurrencyRewardData[] CurrencyDatas { get; private set; }
    [field: SerializeField] public EquipmentRewardData[] EquipmentDatas { get; private set; }
    [field: SerializeField] public InsertRewardData[] InsertDatas { get; private set; }

    public void Validate()
    {
        foreach (var data in CurrencyDatas)
            data.Validate();

        foreach (var data in EquipmentDatas)
            data.Validate();

        foreach (var data in InsertDatas)
            data.Validate();
    }

    public List<AbstractRewardData> GetRewardDatas()
    {
        List<AbstractRewardData> rewardDatas = new List<AbstractRewardData>();

        rewardDatas.AddRange(CurrencyDatas);
        rewardDatas.AddRange(EquipmentDatas);
        rewardDatas.AddRange(InsertDatas);

        return rewardDatas;
    }
}