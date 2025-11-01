using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeTowerButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text tittleText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text unlockLevelText;
    [SerializeField] private Image icon;

    [Space(10), Header("States")]
    [SerializeField] private Transform lockStateView;
    [SerializeField] private Transform bottomView;

    [Space(10), Header("Progress view")]
    [SerializeField] private Slider progressBar;
    [SerializeField] private TMP_Text progressText;
    [SerializeField] private IndicatorAnimator upgradeIndicator;

    private TowersManager towersManager;
    private CurrencyManager currencyManager;
    private UpgradesManager upgradesManager;
    private GeneralSavingData generalSavingData;

    public TowerSO TowerSO { get; private set; }

    public event Action<UpgradeTowerButton> ButtonClicked;

    private void Awake()
    {
        button.onClick.AddListener(OnButtonClicked);
    }

    public void Init(
        TowerSO towerSO,
        TowersManager towersManager,
        CurrencyManager currencyManager,
        UpgradesManager upgradesManager,
        GeneralSavingData generalSavingData)
    {
        TowerSO = towerSO;
        this.towersManager = towersManager;
        this.currencyManager = currencyManager;
        this.upgradesManager = upgradesManager;
        this.generalSavingData = generalSavingData;

        UpdateInfo();
    }

    public void UpdateInfo()
    {
        tittleText.SetText($"{TowerSO.UIData.Name}");
        levelText.SetText($"{towersManager.GetTowerLevel(TowerSO.Id)}");
        unlockLevelText.SetText($"unlock at lv {TowerSO.UnlockLevel}");
        icon.sprite = TowerSO.UIData.Icon;

        UpdateSlider();

        bool isAvailable = generalSavingData.GetParamById(Constants.GLOBAL_PLAYER_LEVEL) >= TowerSO.UnlockLevel;
        lockStateView.gameObject.SetActive(!isAvailable);
        bottomView.gameObject.SetActive(isAvailable);
        button.interactable = isAvailable; 
    }

    private void UpdateSlider()
    {
        UpgradeData upgradeData = upgradesManager.GetUpgradeDataById(TowerSO.Id);

        int targetCurrency = 0;
        int currentCurrency = 0;

        for (int i = 0; i < upgradeData.Datas.Length; i++)
        {
            if (upgradeData.Datas[i].CurrencyType != CurrencyType.Money)
            {
                targetCurrency += upgradesManager.CalculatePrice(upgradeData.Datas[i], towersManager.GetTowerLevel(TowerSO.Id));
                currentCurrency += currencyManager.GetCurrencyAmount(upgradeData.Datas[i].CurrencyType);
            }
        }

        bool isAvialable = currentCurrency >= targetCurrency;

        progressBar.value = currentCurrency / (float)targetCurrency;
        upgradeIndicator.gameObject.SetActive(isAvialable);
        progressText.SetText($"{currentCurrency}/{targetCurrency}");
    }

    private void OnButtonClicked()
    {
        ButtonClicked?.Invoke(this);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnButtonClicked);
    }
}