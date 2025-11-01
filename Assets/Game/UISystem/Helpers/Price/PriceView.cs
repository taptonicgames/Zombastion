using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PriceView : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private HorizontalLayoutGroup horizontalLayoutGroup;

    public void UpdateInfo(Sprite icon, int currentCurrency, int targetCurrency, int level)
    {
        this.icon.sprite = icon;

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