using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndRoundRewadCell : MonoBehaviour
{
    [field: SerializeField] public Transform View { get; private set; }
    [SerializeField] private Image background;
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text text;

    private int rewardValue;

    public void Init(Sprite icon, Sprite bg, int reward)
    {
        this.icon.sprite = icon;
        background.sprite = bg;

        rewardValue = reward;
        this.text.SetText($"{rewardValue}");
    }

    public void IncreaseReward(float modifier)
    {
        rewardValue = Mathf.RoundToInt(rewardValue * modifier);
        text.SetText($"{rewardValue}");
    }
}