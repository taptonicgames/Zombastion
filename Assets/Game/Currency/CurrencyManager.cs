using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Android;
using Zenject;

public class CurrencyManager : IInitializable
{
    [Inject] private SharedObjects sharedObjects;

    private CurrencySO currencySO;

    private Dictionary<CurrencyType, int> currencyValues = new Dictionary<CurrencyType, int>();

    public event Action<CurrencyType> CurrencyChanged;

    public void Initialize()
    {
        currencySO = sharedObjects.GetScriptableObject<CurrencySO>(Constants.CURRENCY_SO);

        //TODO: load saving params
        for (int i = 0; i < currencySO.Datas.Length; i++)
            currencyValues.Add(currencySO.Datas[i].Type, 0);
    }

    public CurrencyData GetCurrencyData(CurrencyType currencyType)
    {
        return currencySO.Datas.FirstOrDefault(d => d.Type == currencyType);
    }

    public int GetCurrencyAmount(CurrencyType currencyType)
    {
        return currencyValues[currencyType];
    }

    public int GetMaxEnergyAmount()
    {
        //TODO: load saving max energy param
        return currencySO.StartMaxEnergy;
    }

    public void AddCurrency(CurrencyType type, int value)
    {
        currencyValues[type] += value;

        CurrencyChanged?.Invoke(type);
    }

    public bool HasPurchased(CurrencyType type, int value)
    {
        return currencyValues[type] >= value;
    }

    public void RemoveCurrency(CurrencyType type, int value)
    {
        currencyValues[type] -= value;

        if (currencyValues[type] < 0)
            throw new ArgumentException($"Currency {type} is below 0, need to check!");

        CurrencyChanged?.Invoke(type);
    }
}