using System;
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
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TMP_Text priceText;

    [Space(10), Header("Card info button")]
    [SerializeField] private TowerCardsInfoPopup cardInfoPopup;
    [SerializeField] private Button cardInfoButton;

    private TowerSO towerSO;
    private BattleUpgradeStorage battleUpgradeStorage;
    private BattleUpgradeConfigsPack upgradeConfigsPack;
    private TowersManager towersManager;

    public event Action Upgraded;

    protected override void Awake()
    {
        base.Awake();

        upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
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

        UpdateInfo();
    }

    private void UpdateInfo()
    {
        tittleText.SetText($"{towerSO.UIData.Name} lv. {towersManager.GetTowerLevel(towerSO.Id)}");
        towerDescription.SetText(towerSO.UIData.Description);
        towerIcon.sprite = towerSO.UIData.Icon;

        InitStats();
    }

    private void InitStats()
    {
        towerStatItemViews[0].Init($"{Constants.STAT_HEALTH_NAME}", $"{CalculateParam(towerSO.Health, towerSO.HealthCoefficient)}");
        towerStatItemViews[2].Init($"{Constants.STAT_DAMAGE_NAME}", $"{CalculateParam(towerSO.Damage, towerSO.ShootDamageCoefficient)}");
        towerStatItemViews[1].Init($"{Constants.STAT_SHOOT_DELAY_NAME}", $"{CalculateParam(towerSO.ShootDelay, towerSO.ShootDelayCoefficient)}");
        towerStatItemViews[3].Init($"{Constants.STAT_CRIT_DAMAGE_NAME}", $"{CalculateParam(towerSO.CritDamage, towerSO.CritDamageCoefficient)}");
        towerStatItemViews[4].Init($"{Constants.STAT_CRIT_PROBALITY_NAME}", $"{CalculateParam(towerSO.CritProbability, towerSO.CritProbabilityCoefficient)}");
        towerStatItemViews[5].gameObject.SetActive(false);
    }

    private float CalculateParam(float paramOne, float paramTwo)
    {
        return towersManager.CalculateParam(towerSO.Id, paramOne, paramTwo);
    }

    private void OnUpgradeButtonClicked()
    {
        towersManager.UpgradeTowerLevel(towerSO.Id);

        UpdateInfo();
    }

    private void OnCardInfoButtonClicked()
    {
        object[] args = new object[] 
        {
            BattleUpgradeType.None,
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
        upgradeButton.onClick.RemoveListener(OnUpgradeButtonClicked);
        cardInfoButton.onClick.RemoveListener(OnCardInfoButtonClicked);
        cardInfoPopup.CloseButtonClicked -= OnPopupClosed;
    }
}