using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class CastleUpgradesPanel : AbstractPanel
{
    [SerializeField] private TMP_Text critValueText;
    [SerializeField] private Transform upgradeTowerItemsContainer;
    [SerializeField] private UpgradeTowerButton upgradeTowerButtonPrefab;
    [SerializeField] private TowerUpgradePopup towerUpgradePopup;

    private List<UpgradeTowerButton> upgradeTowerButtons = new List<UpgradeTowerButton>();
    private TowerSO[] towerSOs;
    private BattleUpgradeConfigsPack upgradeConfigsPack;
    private BattleUpgradeStorage battleUpgradeStorage;
    private GeneralSavingData generalSavingData;

    [Inject] private SharedObjects sharedObjects;
    [Inject] private TowersManager towersManager;
    [Inject] private CurrencyManager currencyManager;
    [Inject] private UpgradesManager upgradesManager;
    [Inject] private AbstractSavingManager savingManager;

    public override PanelType Type => PanelType.CastleUpgrades;

    public override void Init()
    {
        upgradeConfigsPack = sharedObjects.GetScriptableObject<BattleUpgradeConfigsPack>(Constants.BATTLE_UPGRADE_CONFIG_PACK);
        battleUpgradeStorage = new BattleUpgradeStorage(upgradeConfigsPack);
        generalSavingData = savingManager.GetSavingData<GeneralSavingData>(SavingDataType.General);

        towerSOs = towersManager.GetTowerSOs();

        for (int i = 0; i < towerSOs.Length; i++)
        {
            UpgradeTowerButton upgradeTowerButton = Instantiate(upgradeTowerButtonPrefab, upgradeTowerItemsContainer);
            upgradeTowerButton.Init(towerSOs[i], towersManager, currencyManager, upgradesManager, generalSavingData);
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
        foreach (var button in upgradeTowerButtons)
            button.UpdateInfo();

        float critValue = 0;
        for (int i = 0; i < towerSOs.Length; i++)
            critValue += CalculateCritDamage(towerSOs[i]);

        critValueText.SetText($"{critValue}%");
    }

    private float CalculateCritDamage(TowerSO towerSO)
    {
        return towersManager.CalculateParam(towerSO.Id, towerSO.CritDamage, towerSO.CritDamageCoefficient);
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
        object[] args = new object[] 
        {
            button.TowerSO,
            upgradeConfigsPack,
            battleUpgradeStorage,
            towersManager,
            currencyManager,
            upgradesManager
        };

        towerUpgradePopup.Init(args);
        towerUpgradePopup.ForceShow();
    }

    private void OnTowerUpgradePopupClosed()
    {
        UpdateInfo();
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