using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSkillPopup : AbstractPopup
{
    [SerializeField] private TMP_Text tittle;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text valueText;
    [SerializeField] private Image icon;
    [SerializeField] private UpgradeButton upgradeButton;
    [SerializeField] private Transform buttonViewObject;
    [SerializeField] private HorizontalLayoutGroup descriptionlayoutGroup;

    private SkillTreeData data;
    private CurrencyManager currencyManager;
    private UpgradesManager upgradesManager;
    private GeneralSavingData generalSavingData;

    public event Action<SkillTreeData, UpgradeData> SkillOpened;

    protected override void Awake()
    {
        base.Awake();

        upgradeButton.Upgraded += OnButtonClicked;
    }

    public override void Init(object[] args)
    {
        data = (SkillTreeData)args[0];
        bool isShowButton = (bool)args[1];
        currencyManager = (CurrencyManager)args[2];
        upgradesManager = (UpgradesManager)args[3];
        generalSavingData = (GeneralSavingData)args[4];

        tittle.SetText(data.Tittle);
        description.SetText(data.Description);
        valueText.SetText($"{data.Prefix}{data.Value}{data.Postfix}");

        icon.sprite = data.Icon;
        upgradeButton.UpdateInfo(
            upgradesManager.GetUpgradeDataById($"{data.CurrencyType}"),
            currencyManager,
            upgradesManager, 
            generalSavingData.GetParamById(Constants.SKILL_TREE_LEVEL) + 1);

        buttonViewObject.gameObject.SetActive(isShowButton);
    }

    public override void Show(Action callback = null)
    {
        base.Show(() =>
        {
        descriptionlayoutGroup.enabled = false;
        descriptionlayoutGroup.enabled = true;
        });
    }

    private void OnButtonClicked(UpgradeData upgradeData)
    {
        //TODO: implement remove currency

        buttonViewObject.gameObject.SetActive(false);
        SkillOpened?.Invoke(data, upgradeData);
    }

    private void OnDestroy()
    {
        upgradeButton.Upgraded -= OnButtonClicked;
    }
}