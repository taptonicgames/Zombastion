using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class EquipmentManager : IInitializable
{
    [Inject] private EquipmentPackSO equipmentPackSO;
    [Inject] private AbstractSavingManager savingManager;
    [Inject] private RewardsManager rewardsManager;

    private CurrencySavingData currencySavingData;
    private EquipmentSavingData equipmentSavingData;
    private GeneralSavingData generalSavingData;

    private List<EquipmentData> equipmentDatas = new List<EquipmentData>();
    private List<EquipmentData> clothDatas = new List<EquipmentData>();
    private List<InsertData> insertDatas = new List<InsertData>();

    private Dictionary<EquipmentType, string> statNamePairs = new Dictionary<EquipmentType, string>()
    {
        { EquipmentType.Armor, Constants.STAT_HEALTH_NAME},
        { EquipmentType.Helmet, Constants.STAT_CRIT_DAMAGE_NAME},
        { EquipmentType.Weapon, Constants.STAT_DAMAGE_NAME},
        { EquipmentType.Shield, Constants.STAT_ATTACK_SPEED_NAME},
        { EquipmentType.Boots, Constants.STAT_MOVE_SPEED_NAME},
        { EquipmentType.Accessory, Constants.STAT_CRIT_PROBALITY_NAME},
    };

    public void Initialize()
    {
        currencySavingData = savingManager.GetSavingData<CurrencySavingData>(SavingDataType.Currency);
        equipmentSavingData = savingManager.GetSavingData<EquipmentSavingData>(SavingDataType.Equipment);
        generalSavingData = savingManager.GetSavingData<GeneralSavingData>(SavingDataType.General);

        Load();
    }

    private void Load()
    {
        LoadEquip();
        LoadInserts();
        LoadRewards();
    }

    private void LoadEquip()
    {
        for (int i = 0; i < equipmentPackSO.StartEquipments.Length; i++)
        {
            EquipmentType type = equipmentPackSO.StartEquipments[i].Type;

            EquipmentData data = new EquipmentData(
                equipmentSavingData.EquipmentPairs.ContainsKey(type) ?
                GetEquipmentSO(equipmentSavingData.EquipmentPairs[type]) :
                equipmentPackSO.StartEquipments[i]);

            equipmentDatas.Add(data);
        }

        for (int i = 0; i < equipmentSavingData.BagEquipmentPairs.Count; i++)
        {
            for (int j = 0; j < equipmentPackSO.Equipments.Length; j++)
            {
                if (equipmentSavingData.BagEquipmentPairs.ContainsKey(equipmentPackSO.Equipments[j].Id))
                {
                    for (int k = 0; k < equipmentSavingData.BagEquipmentPairs[equipmentPackSO.Equipments[j].Id]; k++)
                    {
                        EquipmentData data = new EquipmentData(equipmentPackSO.Equipments[j]);
                        clothDatas.Add(data);
                    }
                }
            }
        }
    }

    private void LoadInserts()
    {
        for (int i = 0; i < equipmentSavingData.BagInsertPairs.Count; i++)
        {
            for (int j = 0; j < equipmentPackSO.Inserts.Length; j++)
            {
                if (equipmentSavingData.BagInsertPairs.ContainsKey(equipmentPackSO.Inserts[j].Id))
                {
                    for (int k = 0; k < equipmentSavingData.BagInsertPairs[equipmentPackSO.Inserts[j].Id]; k++)
                    {
                        InsertData data = new InsertData(equipmentPackSO.Inserts[j]);
                        insertDatas.Add(data);
                    }
                }
            }
        }

        for (int i = 0; i < equipmentDatas.Count; i++)
        {
            for (int j = 0; j < equipmentPackSO.Inserts.Length; j++)
            {
                if (equipmentSavingData.EquipmentInsertPairs.ContainsKey(equipmentPackSO.Inserts[j].Id))
                {
                    if (equipmentDatas[i].Type == equipmentSavingData.EquipmentInsertPairs[equipmentPackSO.Inserts[j].Id])
                    {
                        InsertData data = new InsertData(equipmentPackSO.Inserts[j]);
                        equipmentDatas[i].SetInsert(data);
                    }
                }
            }
        }
    }

    private void LoadRewards()
    {
        BattleSavingData battleSavingData = savingManager.GetSavingData<BattleSavingData>(SavingDataType.Battle);
        GeneralSavingData generalSavingData = savingManager.GetSavingData<GeneralSavingData>(SavingDataType.General);
        LevelRewardData levelRewardData = rewardsManager.GetLevelRewardData(generalSavingData.GetParamById(Constants.ROUND_PICKED));
        RewardData rewardData = levelRewardData.GetRewardDatasByRoundCompleteType(battleSavingData.RoundCompleteType);

        if (rewardData == null)
            return;

        int rewardApplyAmount = HasIncreaseReward() ? Constants.REWARD_APPLY_AMOUNT : 1;

        for (int i = 0; i < rewardApplyAmount; i++)
            foreach (var reward in rewardData.GetRewardDatas())
                AddEquipment(reward);

        //RewardData[] rewardDatas = levelRewardData.GetRewardDatasByRoundCompleteType(battleSavingData.RoundCompleteType);

        //for (int i = 0; i < rewardDatas.Length; i++)
        //{
        //    int reward = HasIncreaseReward() ? Mathf.RoundToInt(rewardDatas[i].Value * Constants.REWARD_MODIFIER) : rewardDatas[i].Value;

        //    for (int j = 0; j < equipmentPackSO.Equipments.Length; j++)
        //    {
        //        if ($"{rewardDatas[i].CurrencyType}" == equipmentPackSO.Equipments[j].Id)
        //        {
        //            EquipmentData data = new EquipmentData(equipmentPackSO.Equipments[j]);
        //            equipmentSavingData.AddClothData(data);
        //            clothDatas.Add(data);
        //        }
        //    }

        //    for (int j = 0; j < equipmentPackSO.Inserts.Length; j++)
        //    {
        //        if ($"{rewardDatas[i].CurrencyType}" == equipmentPackSO.Inserts[j].Id)
        //        {
        //            InsertData data = new InsertData(equipmentPackSO.Inserts[j]);
        //            equipmentSavingData.AddInsertAtBag(data);
        //            insertDatas.Add(data);
        //        }
        //    }
        //}
    }

    public List<EquipmentData> GetEquipmentDatas()
    {
        return equipmentDatas;
    }

    public List<EquipmentData> GetClothDatas()
    {
        return clothDatas;
    }

    public List<InsertData> GetInsertDatas()
    {
        return insertDatas;
    }

    public EquipmentData GetEquipmentDataByType(EquipmentType type)
    {
        return equipmentDatas.FirstOrDefault(i => i.Type == type);
    }

    public int CalculateDamage(int defaultValue)
    {
        return CalculateParam(EquipmentType.Weapon, defaultValue);
    }

    public string GetStatName(EquipmentType type)
    {
        return statNamePairs[type];
    }

    public void SetEquipment(EquipmentData data)
    {
        equipmentSavingData.AddEquipment(data.Type, data.Id);
    }

    public void EquipInsert(InsertData insertData, EquipmentType equipmentType)
    {
        equipmentSavingData.EquipInsert(insertData, equipmentType);
        equipmentSavingData.RemoveInsertAtBag(insertData);
    }

    public void UnequipInsert(InsertData insertData)
    {
        equipmentSavingData.UnequipInsert(insertData);
        equipmentSavingData.AddInsertAtBag(insertData);
    }

    public void UpdateEquipmentData(EquipmentData currentEquipmentData, EquipmentData targetEquipmentData)
    {
        for (int i = 0; i < currentEquipmentData.InsertDatas.Length; i++)
        {
            targetEquipmentData.SetInsert(currentEquipmentData.InsertDatas[i]);
        }

        equipmentDatas.Remove(currentEquipmentData);
        equipmentDatas.Add(targetEquipmentData);
    }

    public bool HasIncreaseReward()
    {
        return currencySavingData.HasIncreaseReward();
    }

    public void AddEquipment(AbstractRewardData abstractRewardData)
    {
        if (abstractRewardData is EquipmentRewardData equipment)
        {
            EquipmentData data = new EquipmentData(GetEquipmentSO(equipment.Id));
            equipmentSavingData.AddClothData(data);
            clothDatas.Add(data);
        }
        else if (abstractRewardData is InsertRewardData insert)
        {
            InsertData data = new InsertData(GetInsertSO(insert.Id));
            equipmentSavingData.AddInsertAtBag(data);
            insertDatas.Add(data);
        }
    }

    private EquipmentSO GetEquipmentSO(string id)
    {
        return equipmentPackSO.Equipments.FirstOrDefault(i => i.Id == id);
    }

    private InsertSO GetInsertSO(string id)
    { 
        return equipmentPackSO.Inserts.FirstOrDefault(i => i.Id == id);
    }

    private int CalculateParam(EquipmentType equipmentType, int defaultValue)
    {
        EquipmentData data = equipmentDatas.FirstOrDefault(d => d.Type == equipmentType);
        Debug.LogWarning(data.Id);
        Debug.LogWarning($"{defaultValue + data.Value + GetModParam(defaultValue, data)} * {1 + generalSavingData.GetParamById(Constants.GLOBAL_PLAYER_LEVEL) * data.GrowFactor}");
        Debug.LogWarning($"({defaultValue} + {data.Value} + {GetModParam(defaultValue, data)}) * ({1} + {generalSavingData.GetParamById(Constants.GLOBAL_PLAYER_LEVEL)} * {data.GrowFactor})");

        return
            Mathf.RoundToInt((defaultValue + data.Value + GetModParam(defaultValue, data) *
            (1 + generalSavingData.GetParamById(Constants.GLOBAL_PLAYER_LEVEL)) * data.GrowFactor));

        //HP = (BaseHP + EquipmentHP + ModHP) × (1 + Level × GrowthFactorHP)
    }

    private int GetModParam(int defaultValue, EquipmentData data)
    {
        float percentage = 0;

        for (int i = 0; i < data.InsertDatas.Length; i++)
        {
            if (data.InsertDatas[i] != null)
                percentage += data.InsertDatas[i].PercentageBonus;
        }

        return (defaultValue + Mathf.RoundToInt(defaultValue * percentage));
    }
}