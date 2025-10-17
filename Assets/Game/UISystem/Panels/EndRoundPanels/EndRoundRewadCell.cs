using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndRoundRewadCell : MonoBehaviour
{
    [field: SerializeField] public Transform View { get; private set; }
    [SerializeField] private Image background;
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text text;

    public void Show(Sprite sprite)
    {
        icon.sprite = sprite;
    }

    public void SetTextValue(int targetValue, int queue, string prefix = "", int startValue = 0)
    {
        text.SetText($"{targetValue}");
    }
}