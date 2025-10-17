using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CastleUpgradesPanel : AbstractPanel
{
    [SerializeField] private TMP_Text critValueText;
    [SerializeField] private Transform upgradeTowerItemsContainer;
    [SerializeField] private UpgradeTowerButton upgradeTowerButtonPrefab;
    [SerializeField] private TowerUpgradePopup towerUpgradePopup;

    private List<UpgradeTowerButton> upgradeTowerButtons = new List<UpgradeTowerButton>();

    public override PanelType Type => PanelType.CastleUpgrades;

    public override void Init()
    {
        for (int i = 0; i < 10; i++)
        {
            UpgradeTowerButton upgradeTowerButton = Instantiate(upgradeTowerButtonPrefab, upgradeTowerItemsContainer);
            upgradeTowerButton.Init();
            upgradeTowerButtons.Add(upgradeTowerButton);
        }

        Subscribe();

        towerUpgradePopup.ForceHide();
    }

    public override async UniTask OnShow()
    {
        await UniTask.Yield();

        UpdateInfo();
    }

    private void UpdateInfo()
    {
        // TODO: show tower datas info
        critValueText.SetText($"{10}%");
    }

    #region Events
    private void Subscribe()
    {
        foreach (var button in upgradeTowerButtons)
            button.ButtonClicked += OnTowerUpgradeButtonClicked;

        towerUpgradePopup.CloseButtonClicked += OnTowerUpgradePopupClosed;
        towerUpgradePopup.Upgraded += OnTowerUpgraded;
    }

    private void Unsubscribe()
    {
        foreach (var button in upgradeTowerButtons)
            button.ButtonClicked -= OnTowerUpgradeButtonClicked;

        towerUpgradePopup.CloseButtonClicked -= OnTowerUpgradePopupClosed;
        towerUpgradePopup.Upgraded -= OnTowerUpgraded;
    }

    private void OnTowerUpgradeButtonClicked(UpgradeTowerButton button)
    {
        // TODO: implement tower data
        object[] args = new object[] {button};
        towerUpgradePopup.Init(args);
        towerUpgradePopup.ForceShow();
    }

    private void OnTowerUpgradePopupClosed()
    {
        towerUpgradePopup.ForceHide();
    }

    private void OnTowerUpgraded()
    {
        UpdateInfo();
    }
    #endregion
    
    private void OnDestroy()
    {
        Unsubscribe();
    }
}