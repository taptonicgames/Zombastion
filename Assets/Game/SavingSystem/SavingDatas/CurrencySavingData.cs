using System.Collections.Generic;
using UnityEngine;

public class CurrencySavingData : AbstractSavingData
{
    private int defaultMaxEnergy;
    [field: SerializeField] public bool IsIncreaseReward { get; set; }
    [field: SerializeField] public Dictionary<CurrencyType, int> CurrencyPairs { get; set; } = new Dictionary<CurrencyType, int>();

    public override void ResetData(int flag = 0)
    {
        IsIncreaseReward = false;
    }

    public void Init(int defaultMaxEnergy)
    {
        this.defaultMaxEnergy = defaultMaxEnergy;
    }

    public int GetCurrencyById(CurrencyType currencyType)
    {
        if (CurrencyPairs.ContainsKey(currencyType) == false)
        {
            int value = (currencyType == CurrencyType.MaxEnergy || currencyType == CurrencyType.Energy) ? defaultMaxEnergy : 0;
            CurrencyPairs.Add(currencyType, value);
            SaveData(false);
        }

        return CurrencyPairs[currencyType];
    }

    public void SetCurrencyById(CurrencyType currencyType, int value)
    {
        CurrencyPairs[currencyType] = value;
        SaveData(false);
    }

    public void SetIncreaseReward(bool isIncrease)
    {
        IsIncreaseReward = isIncrease;
        SaveData(false);
    }

    public bool HasIncreaseReward()
    {
        return IsIncreaseReward;
    }

    public override void SaveData(bool collectParams, bool isSave = true)
    {
        if (isSave == false) return;

        base.SaveData(collectParams);
    }

    protected override void SaveDataObject()
    {
        ES3.Save(ToString(), this);
    }
}