using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndRoundStatItemView : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private TMP_Text dpsText;

    public void Init(Sprite sprite, float damage, float dps)
    {
        icon.sprite = sprite;
        damageText.SetText($"{damage}");
        dpsText.SetText($"{dps}");
    }
}