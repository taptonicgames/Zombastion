using log4net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class EnhancePopup : AbstractPopup
{
    [Header("Item view")]
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text tittleText;

    [Space(10), Header("Levels view")]
    [SerializeField] private TMP_Text currentLevelText;
    [SerializeField] private TMP_Text nextLevelText;

    [Space(10), Header("Stats view")]
    [SerializeField] private TMP_Text currentStatsText;
    [SerializeField] private TMP_Text nextStatsText;

    [Space(10), Header("Buttons")]
    [SerializeField] private UpgradeButton upgradeButton;
    [SerializeField] private Button enhanceAllButton;

    [Space(10), Header("Equipment content")]
    [SerializeField] private EquipmentButton equipmentButtonPrefab;
    [SerializeField] private RectTransform buttonsContainer;

    private EquipmentData currentData;
    private CurrencyManager currencyManager;
    private UpgradesManager upgradesManager;

    private List<EquipmentButton> equipmentButtons = new List<EquipmentButton>();

    [Inject] private EquipmentManager equipmentManager;

    protected override void Awake()
    {
        base.Awake();

        upgradeButton.Upgraded += OnEnhanceButtonClicked;
        enhanceAllButton.onClick.AddListener(OnEnhanceAllButtonClicked);

        ForceHide();
    }

    public override void Init(object[] args)
    {
        ClothItemView itemView = (ClothItemView)args[0];
        currencyManager = (CurrencyManager)args[1];
        upgradesManager = (UpgradesManager)args[2];

        currentData = itemView.EquipmentData;

        if (equipmentButtons.Count == 0)
            CreateButtonsList();

        UpdateInfo();
    }

    private void CreateButtonsList()
    {
        List<EquipmentData> datas = equipmentManager.GetEquipmentDatas();

        foreach (var data in datas)
        {
            EquipmentButton equipmentButton = Instantiate(equipmentButtonPrefab, buttonsContainer);
            equipmentButton.Init(data);
            equipmentButton.ChangePickedState(false);
            equipmentButton.ButtonClicked += OnButtonClicked;
            equipmentButtons.Add(equipmentButton);
        }
    }

    private void OnButtonClicked(EquipmentButton button)
    {
        currentData = button.Data;
        UpdateInfo();
    }

    private void UpdateInfo()
    {
        foreach (var button in equipmentButtons)
        {
            button.UpdateInfo();
            button.ChangePickedState(button.Data.Id == currentData.Id);
        }

        icon.sprite = currentData.UIData.Icon;
        tittleText.SetText($"{currentData.Id}");
        levelText.SetText($"Lv. {currentData.Level}");

        currentLevelText.SetText($"Lv. {currentData.Level}");
        nextLevelText.SetText($"Lv. {currentData.Level + 1}");

        currentStatsText.SetText($"ATK +{currentData.EnchanceLevels[currentData.Level]}");
        nextStatsText.SetText($"ATK +{currentData.EnchanceLevels[currentData.Level + 1]}");

        upgradeButton.UpdateInfo(
            upgradesManager.GetUpgradeDataById($"{currentData.UpgradeCurrency}"),
            currencyManager,
            upgradesManager,
            currentData.Level + 1);
    }

    private void OnEnhanceButtonClicked(UpgradeData upgradeData)
    {
        for (int i = 0; i < upgradeData.Datas.Length; i++)
            currencyManager.RemoveCurrency(
                upgradeData.Datas[i].CurrencyType,
                upgradesManager.CalculatePrice(upgradeData.Datas[i], currentData.Level));

        currentData.UpgradeLevel();

        UpdateInfo();
    }

    private void OnEnhanceAllButtonClicked()
    {
        List<EquipmentData> datas = equipmentManager.GetEquipmentDatas();

        foreach (var data in datas)
            data.UpgradeLevel();

        UpdateInfo();
    }

    private void OnDestroy()
    {
        upgradeButton.Upgraded -= OnEnhanceButtonClicked;
        enhanceAllButton.onClick.RemoveListener(OnEnhanceAllButtonClicked);
    }
}
