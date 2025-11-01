using System;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private PriceView[] priceViews;
    [SerializeField] private HorizontalLayoutGroup horizontalLayoutGroup;

    private UpgradeData upgradeData;
    private UpgradesManager upgradesManager;

    public Action<UpgradeData> Upgraded;

    private void Awake()
    {
        button.onClick.AddListener(OnButtonClicked);
    }

    public void UpdateInfo(
        UpgradeData upgradeData,
        CurrencyManager currencyManager,
        UpgradesManager upgradesManager,
        SpritesManager spritesManager,
        int level)
    {
        this.upgradeData = upgradeData;
        this.upgradesManager = upgradesManager;

        for (int i = 0; i < priceViews.Length; i++)
        {
            priceViews[i].gameObject.SetActive(i < upgradeData.Datas.Length);

            if (priceViews[i].gameObject.activeSelf)
            {
                Sprite icon = spritesManager.GetIconSprite(new CurrencySpritesData(upgradeData.Datas[i].CurrencyType));
                int currentCurrency = currencyManager.GetCurrencyAmount(upgradeData.Datas[i].CurrencyType);
                int targetCurrency = GetTargetCurrency(upgradeData.Datas[i], level);

                priceViews[i].UpdateInfo(icon, currentCurrency,  targetCurrency, level);
            }
        }
        
        button.interactable = HasPurchased(currencyManager, upgradesManager, level);
    }

    private bool HasPurchased(CurrencyManager currencyManager, UpgradesManager upgradesManager, int level)
    {
        for(int i = 0; i < upgradeData.Datas.Length; i++)
            if (currencyManager.HasPurchased(upgradeData.Datas[i].CurrencyType, GetTargetCurrency(upgradeData.Datas[i], level)) == false)
                return false;

        return true;
    }

    private int GetTargetCurrency(PriceData priceData, int level)
    {
        int currency = upgradesManager.CalculatePrice(priceData, level);
        return currency;
    }

    private void OnButtonClicked()
    {
        Upgraded?.Invoke(upgradeData);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnButtonClicked);
    }
}
