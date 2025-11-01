using System.Linq;
using UnityEngine;
using Zenject;

public class UpgradesManager : IInitializable
{
    [Inject] private SharedObjects sharedObjects;

    private const string UPGRADES_CONFIG = "UpgradesConfig";

    private UpgradesSO upgradeSO;

    public void Initialize()
    {
        upgradeSO = sharedObjects.GetScriptableObject<UpgradesSO>(UPGRADES_CONFIG);
    }

    public UpgradeData GetUpgradeDataById(string id)
    {
        return upgradeSO.Datas.FirstOrDefault(u => u.Id == id);
    }

    public int CalculatePrice(PriceData priceData, int level)
    {
        return Mathf.RoundToInt(
            level > 1 ?
            priceData.StartPrice * level * priceData.PriceModifier :
            priceData.StartPrice);
    }
}