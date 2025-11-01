using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerUpgradePopup : AbstractPopup
{
    [Space(10), Header("Tower view")]
    [SerializeField] private TMP_Text tittleText;
    [SerializeField] private Image towerIcon;
    [SerializeField] private TMP_Text towerDescription;

    [Space(10), Header("Tower stats")]
    [SerializeField] private TowerStatItemView[] towerStatItemViews;

    [Space(10), Header("Tower skills")]
    [SerializeField] private Transform towerSkillItemViewsContainer;
    [SerializeField] private TowerSkillItemView towerSkillItemViewPrefab;

    [Space(10), Header("Upgrade view")]
    [SerializeField] private TMP_Text upgradeText;
    [SerializeField] private UpgradeButton upgradeButton;
    [SerializeField] private UpgradesSO[] upgradeSOs;

    [Space(10), Header("Card info button")]
    [SerializeField] private TowerCardsInfoPopup cardInfoPopup;
    [SerializeField] private Button cardInfoButton;

    private TowerSO towerSO;
    private BattleUpgradeStorage battleUpgradeStorage;
    private BattleUpgradeConfigsPack upgradeConfigsPack;
    private TowersManager towersManager;
    private CurrencyManager currencyManager;
    private UpgradesManager upgradesManager;
    private SpritesManager spritesManager;

    public event Action Upgraded;

    protected override void Awake()
    {
        base.Awake();

        upgradeButton.Upgraded += OnUpgradeButtonClicked;
        cardInfoButton.onClick.AddListener(OnCardInfoButtonClicked);
        cardInfoPopup.CloseButtonClicked += OnPopupClosed;

        cardInfoPopup.ForceHide();
    }

    public override void Init(object[] args)
    {
        towerSO = (TowerSO)args[0];
        upgradeConfigsPack = (BattleUpgradeConfigsPack)args[1];
        battleUpgradeStorage = (BattleUpgradeStorage)args[2];
        towersManager = (TowersManager)args[3];
        currencyManager = (CurrencyManager)args[4];
        upgradesManager = (UpgradesManager)args[5];
        spritesManager = (SpritesManager)args[6];

        UpdateInfo();
    }

    private void UpdateInfo()
    {
        int level = towersManager.GetTowerLevel(towerSO.Id);
        tittleText.SetText($"{towerSO.UIData.Name} lv. {level}");
        towerDescription.SetText(towerSO.UIData.Description);
        towerIcon.sprite = towerSO.UIData.Icon;
        upgradeButton.UpdateInfo(GetUpgradeSO(), currencyManager, upgradesManager, spritesManager, level);

        InitStats();
    }

    private void InitStats()
    {
        towerStatItemViews[0].Init($"{Constants.STAT_HEALTH_NAME}", $"{CalculateParam(towerSO.Health, towerSO.HealthCoefficient)}");
        towerStatItemViews[1].Init($"{Constants.STAT_DAMAGE_NAME}", $"{CalculateParam(towerSO.Damage, towerSO.ShootDamageCoefficient)}");
        towerStatItemViews[2].Init($"{Constants.STAT_SHOOT_DELAY_NAME}", $"{CalculateParam(towerSO.ShootDelay, towerSO.ShootDelayCoefficient)}");
        towerStatItemViews[3].Init($"{Constants.STAT_CRIT_DAMAGE_NAME}", $"{CalculateParam(towerSO.CritDamage, towerSO.CritDamageCoefficient)}");
        towerStatItemViews[4].Init($"{Constants.STAT_CRIT_PROBALITY_NAME}", $"{CalculateParam(towerSO.CritProbability, towerSO.CritProbabilityCoefficient)}");
        towerStatItemViews[5].gameObject.SetActive(false);
    }

    private float CalculateParam(float paramOne, float paramTwo)
    {
        return towersManager.CalculateParam(towerSO.Id, paramOne, paramTwo);
    }

    private UpgradeData GetUpgradeSO()
    {
        return upgradesManager.GetUpgradeDataById(towerSO.Id);
    }

    private void OnUpgradeButtonClicked(UpgradeData upgradeData)
    {
        int level = towersManager.GetTowerLevel(towerSO.Id);

        for (int i = 0; i < upgradeData.Datas.Length; i++)
            currencyManager.RemoveCurrency(
                upgradeData.Datas[i].CurrencyType,
                upgradesManager.CalculatePrice(upgradeData.Datas[i],
                level));

        towersManager.UpgradeTowerLevel(towerSO.Id);

        UpdateInfo();
    }

    private void OnCardInfoButtonClicked()
    {
        object[] args = new object[]
        {
            BattleUpgradeType.Stats,
            towerSO.WeaponType,
            $"{towerSO.UIData.Name} lv. {towersManager.GetTowerLevel(towerSO.Id)}",
            upgradeConfigsPack,
            battleUpgradeStorage
        };

        cardInfoPopup.Init(args);
        cardInfoPopup.ForceShow();
    }

    private void OnPopupClosed()
    {
        cardInfoPopup.ForceHide();
    }

    private void OnDestroy()
    {
        upgradeButton.Upgraded -= OnUpgradeButtonClicked;
        cardInfoButton.onClick.RemoveListener(OnCardInfoButtonClicked);
        cardInfoPopup.CloseButtonClicked -= OnPopupClosed;
    }
}