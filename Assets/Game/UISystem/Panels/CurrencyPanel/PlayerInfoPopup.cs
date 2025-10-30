using System;
using TMPro;
using UnityEngine;

public class PlayerInfoPopup : AbstractPopup
{
    [SerializeField] private UpgradeButton upgradeButton;
    [SerializeField] private TMP_Text tittleText;

    private GeneralSavingData generalSavingData;
    private CurrencyManager currencyManager;
    private UpgradesManager upgradesManager;

    public event Action<UpgradeData> Upgraded;

    protected override void Awake()
    {
        base.Awake();

        upgradeButton.Upgraded += OnButtonClicked;
    }

    public override void Init(object[] args)
    {
        generalSavingData = (GeneralSavingData)args[0];
        currencyManager = (CurrencyManager)args[1];
        upgradesManager = (UpgradesManager)args[2];

        UpdateInfo();
    }

    private void UpdateInfo()
    {
        int level = generalSavingData.GetParamById(Constants.GLOBAL_PLAYER_LEVEL);
        tittleText.SetText($"player level {level}");

        upgradeButton.UpdateInfo(
            upgradesManager.GetUpgradeDataById(Constants.GLOBAL_PLAYER_LEVEL),
            currencyManager,
            upgradesManager,
            level);
    }

    private void OnButtonClicked(UpgradeData upgradeData)
    {
        Upgraded?.Invoke(upgradeData);

        UpdateInfo();
    }

    private void OnDestroy()
    {
        upgradeButton.Upgraded -= OnButtonClicked;
    }
}
