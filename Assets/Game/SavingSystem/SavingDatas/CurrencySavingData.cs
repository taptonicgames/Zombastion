using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CurrencySavingData : AbstractSavingData
{
    private int defaultMaxEnergy;
    public Dictionary<CurrencyType, int> currencyPairs = new Dictionary<CurrencyType, int>();

    public override void ResetData(int flag = 0)
    {
    }

    public void Init(int defaultMaxEnergy)
    {
        this.defaultMaxEnergy = defaultMaxEnergy;
    }

    public int GetCurrencyById(CurrencyType currencyType)
    {
        if (currencyPairs.ContainsKey(currencyType) == false)
        {
            int value = currencyType == CurrencyType.MaxEnergy ? defaultMaxEnergy : 0;
            currencyPairs.Add(currencyType, value);
            SaveData(false);
        }

        return currencyPairs[currencyType];
    }

    public void SetCurrencyById(CurrencyType currencyType, int value)
    {
        currencyPairs[currencyType] = value;
        SaveData(false);
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