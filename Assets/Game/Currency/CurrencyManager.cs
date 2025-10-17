using System.Collections;
using System.Linq;
using UnityEngine;
using Zenject;

public class CurrencyManager : IInitializable
{
    [Inject] private SharedObjects sharedObjects;

    private CurrencySO levelRewardsSO;

    public void Initialize()
    {
        levelRewardsSO = sharedObjects.GetScriptableObject<CurrencySO>(Constants.CURRENCY_SO);
    }

    public CurrencyData GetCurrencyData(CurrencyType currencyType)
    {
        return levelRewardsSO.Datas.FirstOrDefault(d => d.Type == currencyType);
    }
}