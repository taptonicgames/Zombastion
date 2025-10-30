using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SkillsTreePanel : AbstractPanel
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform levelsContainer;
    [SerializeField] private TreeSkillsLevelView treeSkillsLevelViewPrefab;
    [SerializeField] private Button closeButton;
    [SerializeField] private LevelSkillPopup levelSkillPopup;

    private List<TreeSkillsLevelView> levelViews = new List<TreeSkillsLevelView>();

    [Inject] private SkillsTreeSO skillsTreeSO;
    [Inject] private AbstractSavingManager savingManager;
    [Inject] private CurrencyManager currencyManager;
    [Inject] private UpgradesManager upgradesManager;

    private GeneralSavingData generalSavingData;

    public override PanelType Type => PanelType.SkillsTree;

    public override void Init()
    {
        generalSavingData = savingManager.GetSavingData<GeneralSavingData>(SavingDataType.General);

        double count = Math.Ceiling((double)skillsTreeSO.Datas.Length / skillsTreeSO.SkillsCountPerLevel);
        for (int i = 0; i < count; i++)
        {
            TreeSkillsLevelView treeSkillsLevelView = Instantiate(treeSkillsLevelViewPrefab, levelsContainer);
            treeSkillsLevelView.Init(i + 1);

            if (i < generalSavingData.GetParamById(Constants.GLOBAL_PLAYER_LEVEL) - 1)
                treeSkillsLevelView.Unlock();
            else
                treeSkillsLevelView.Lock();

            levelViews.Add(treeSkillsLevelView);
        }

        int counter = 0;
        foreach (var levelView in levelViews)
        {
            for (int i = 0; i < skillsTreeSO.SkillsCountPerLevel; i++)
            {
                if (counter < skillsTreeSO.Datas.Length)
                {
                    levelView.AddSkill(skillsTreeSO.Datas[counter]);
                    counter++;
                }
            }
        }

        UpdatePanel();

        Subscrube();

        levelSkillPopup.ForceHide();
    }

    public override async UniTask OnShow()
    {
        await UniTask.Yield();
        scrollRect.VerticalScrollToTarget(levelsContainer.GetComponent<RectTransform>(), levelViews[0].RectTransform);

        UpdatePanel();
    }

    private void UpdatePanel()
    {
        for (int i = 0; i < levelViews.Count; i++)
        {
            if (i < generalSavingData.GetParamById(Constants.GLOBAL_PLAYER_LEVEL) - 1)
                levelViews[i].Unlock();
            else
                levelViews[i].Lock();
        }

        int counter = 0;
        for (int i = 0; i < levelViews.Count; i++)
        {
            for (int j = 0; j < levelViews[i].Buttons.Count; j++)
            {
                levelViews[i].Buttons[j].ChangeReceivedState(counter < generalSavingData.GetParamById(Constants.SKILL_TREE_LEVEL));

                if (counter < generalSavingData.GetParamById(Constants.SKILL_TREE_LEVEL))
                    counter++;
            }
        }
    }

    #region Events
    private void Subscrube()
    {
        closeButton.onClick.AddListener(OnCloseButtonClicked);
        levelSkillPopup.CloseButtonClicked += OnPopupClosed;
        levelSkillPopup.SkillOpened += OnOnSkillOpened;
        foreach (var levelView in levelViews)
            levelView.ButtonClicked += OnButtonClicked;
    }

    private void Unsubscribe()
    {
        closeButton.onClick.RemoveListener(OnCloseButtonClicked);
        levelSkillPopup.CloseButtonClicked -= OnPopupClosed;
        levelSkillPopup.SkillOpened -= OnOnSkillOpened;
        foreach (var levelView in levelViews)
            levelView.ButtonClicked -= OnButtonClicked;
    }

    private void OnCloseButtonClicked()
    {
        EventBus<OpenPanelEvnt>.Publish(
                        new OpenPanelEvnt() { type = PanelType.PlayerUpgrades });
    }

    private void OnPopupClosed()
    {
        levelSkillPopup.Hide();
    }

    private void OnOnSkillOpened(SkillTreeData data, UpgradeData upgradeData)
    {
        int level = generalSavingData.GetParamById(Constants.SKILL_TREE_LEVEL);

        for (int i = 0; i < upgradeData.Datas.Length; i++)
            currencyManager.RemoveCurrency(upgradeData.Datas[i].CurrencyType, upgradesManager.CalculatePrice(upgradeData.Datas[i], level));

        generalSavingData.SetParamById(Constants.SKILL_TREE_LEVEL, level + 1);

        UpdatePanel();
    }

    private void OnButtonClicked(LevelSkillButton button)
    {
        bool isHasShowedPurchaseButton = HasShowedPurchaseButton(button);
        object[] args = new object[] 
        { 
            button.Data,
            isHasShowedPurchaseButton,
            currencyManager,
            upgradesManager,
            generalSavingData
        };

        levelSkillPopup.Init(args);
        levelSkillPopup.Show();
    }
    #endregion

    private bool HasShowedPurchaseButton(LevelSkillButton button)
    {
        for (int i = 0; i < skillsTreeSO.Datas.Length; i++)
            if (button.Data == skillsTreeSO.Datas[i] && i == generalSavingData.GetParamById(Constants.SKILL_TREE_LEVEL))
                return true;

        return false;
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }
}