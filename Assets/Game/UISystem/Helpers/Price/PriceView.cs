using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PriceView : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private HorizontalLayoutGroup horizontalLayoutGroup;

    public void UpdateInfo(PriceData priceData, CurrencyManager currencyManager, int targetCurrency, int level)
    {
        CurrencyData currencyData = currencyManager.GetCurrencyData(priceData.CurrencyType);
        int currentCurrency = currencyManager.GetCurrencyAmount(priceData.CurrencyType);

        this.icon.sprite = currencyData.UIData.Icon;

        string color = currentCurrency >= targetCurrency ? "green" : "red";
        priceText.SetText($"<color={color}>{currentCurrency}</color>/{targetCurrency}");

        UpdateLayoutGroup().Forget();
    }

    private async UniTask UpdateLayoutGroup()
    {
        horizontalLayoutGroup.enabled = false;
        await UniTask.Yield();
        horizontalLayoutGroup.enabled = true;
    }
}