using System;
using UnityEngine;
using System.Linq;
using Zenject;

public class CurrencyManager : IInitializable
{
    [Inject] private SharedObjects sharedObjects;
    [Inject] private AbstractSavingManager savingManager;
    [Inject] private RewardsManager rewardsManager;

    private CurrencySO currencySO;
    private CurrencySavingData currencySavingData;

    public event Action<CurrencyType> CurrencyChanged;

    public void Initialize()
    {
        currencySO = sharedObjects.GetScriptableObject<CurrencySO>(Constants.CURRENCY_SO);
        currencySavingData = savingManager.GetSavingData<CurrencySavingData>(SavingDataType.Currency);
        currencySavingData.Init(currencySO.StartMaxEnergy);

        LoadRewards();
    }

    public void LoadRewards()
    {
        BattleSavingData battleSavingData = savingManager.GetSavingData<BattleSavingData>(SavingDataType.Battle);
        GeneralSavingData generalSavingData = savingManager.GetSavingData<GeneralSavingData>(SavingDataType.General);
        LevelRewardData levelRewardData = rewardsManager.GetLevelRewardData(generalSavingData.GetParamById(Constants.ROUND_PICKED));
        RewardData[] rewardDatas = levelRewardData.GetRewardDatasByRoundCompleteType(battleSavingData.RoundCompleteType);

        for (int i = 0; i < rewardDatas.Length; i++)
        {
            int reward = HasIncreaseReward() ? Mathf.RoundToInt(rewardDatas[i].Value * Constants.REWARD_MODIFIER) : rewardDatas[i].Value;
            AddCurrency(rewardDatas[i].CurrencyType, reward);
        }

        currencySavingData.ResetData();
    }

    public CurrencyData GetCurrencyData(CurrencyType currencyType)
    {
        if (currencySO == null)
            currencySO = sharedObjects.GetScriptableObject<CurrencySO>(Constants.CURRENCY_SO);

        return currencySO.Datas.FirstOrDefault(d => d.Type == currencyType);
    }

    public int GetCurrencyAmount(CurrencyType currencyType)
    {
        return currencySavingData.GetCurrencyById(currencyType);
    }

    public void AddCurrency(CurrencyType type, int value)
    {
        int newValue = currencySavingData.GetCurrencyById(type) + value;
        currencySavingData.SetCurrencyById(type, newValue);

        CurrencyChanged?.Invoke(type);
    }

    public bool HasPurchased(CurrencyType type, int value)
    {
        return currencySavingData.GetCurrencyById(type) >= value;
    }

    public void RemoveCurrency(CurrencyType type, int value)
    {
        int newValue = currencySavingData.GetCurrencyById(type) - value;
        currencySavingData.SetCurrencyById(type, newValue);

        if (newValue < 0)
            throw new ArgumentException($"Currency {type} is below 0, need to check!");

        CurrencyChanged?.Invoke(type);
    }

    public void SetIncreaseReward(bool isIncrease)
    {
        if (currencySavingData == null)
            currencySavingData = savingManager.GetSavingData<CurrencySavingData>(SavingDataType.Currency);

        currencySavingData.SetIncreaseReward(isIncrease);
    }

    public bool HasIncreaseReward()
    {
        return currencySavingData.HasIncreaseReward();
    }
}