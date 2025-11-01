using System;
using Zenject;

public class CurrencyManager : IInitializable
{
    [Inject] private AbstractSavingManager savingManager;
    [Inject] private RewardsManager rewardsManager;

    private CurrencySavingData currencySavingData;

    private const int START_MAX_ENERGY = 30;

    public event Action<CurrencyType> CurrencyChanged;

    public void Initialize()
    {
        currencySavingData = savingManager.GetSavingData<CurrencySavingData>(SavingDataType.Currency);
        currencySavingData.Init(START_MAX_ENERGY);

        LoadRewards();
    }

    public void LoadRewards()
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
                AddCurrency(reward);

        currencySavingData.ResetData();
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

    public void AddCurrency(AbstractRewardData rewardData)
    {
        if (rewardData is CurrencyRewardData currency)
            AddCurrency(currency.Type, currency.Value);
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