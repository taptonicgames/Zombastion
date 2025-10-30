using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProfileButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image proggressFill;
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text levelText;

    private GeneralSavingData generalSavingData;
    private CurrencyManager currencyManager;

    public event Action ProfileButtonClicked;

    private void Awake()
    {
        button.onClick.AddListener(OnButtonClicked);
    }

    public void Init(GeneralSavingData generalSavingData, CurrencyManager currencyManager)
    {
        this.generalSavingData = generalSavingData;
        this.currencyManager = currencyManager;

        UpdateInfo();
    }

    public void UpdateInfo()
    {
        nameText.SetText($"profile name");
        levelText.SetText($"{generalSavingData.GetParamById(Constants.GLOBAL_PLAYER_LEVEL)}");
        proggressFill.fillAmount = 0;
    }

    private void OnButtonClicked()
    {
        ProfileButtonClicked?.Invoke();
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnButtonClicked);
    }
}