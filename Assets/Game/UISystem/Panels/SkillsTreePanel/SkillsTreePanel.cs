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

    //TODO: implement global player level
    private int testFGlobalPlayerLevel;
    //TODO: implement tree skills level
    private int testTreeSkillsLevel;

    private List<TreeSkillsLevelView> levelViews = new List<TreeSkillsLevelView>();

    [Inject] private SkillsTreeSO skillsTreeSO;

    public override PanelType Type => PanelType.SkillsTree;

    public override void Init()
    {
        double count = Math.Ceiling((double)skillsTreeSO.Datas.Length / skillsTreeSO.SkillsCountPerLevel);
        for (int i = 0; i < count; i++)
        {
            TreeSkillsLevelView treeSkillsLevelView = Instantiate(treeSkillsLevelViewPrefab, levelsContainer);
            treeSkillsLevelView.Init(i + 1);

            if (i < testFGlobalPlayerLevel)
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
    }

    private void UpdatePanel()
    {
        for (int i = 0; i < levelViews.Count; i++)
        {
            if (i < testFGlobalPlayerLevel)
                levelViews[i].Unlock();
            else
                levelViews[i].Lock();
        }

        int counter = 0;
        for (int i = 0; i < levelViews.Count; i++)
        {
            for (int j = 0; j < levelViews[i].Buttons.Count; j++)
            {
                levelViews[i].Buttons[j].ChangeReceivedState(counter < testTreeSkillsLevel);

                if (counter < testTreeSkillsLevel)
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

    private void OnOnSkillOpened(SkillTreeData data)
    {
        testTreeSkillsLevel++;

        UpdatePanel();
    }

    private void OnButtonClicked(LevelSkillButton button)
    {
        bool isHasShowedPurchaseButton = HasShowedPurchaseButton(button);
        object[] args = new object[] { button.Data, isHasShowedPurchaseButton };
        levelSkillPopup.Init(args);
        levelSkillPopup.Show();
    }
    #endregion

    private bool HasShowedPurchaseButton(LevelSkillButton button)
    {
        for (int i = 0; i < skillsTreeSO.Datas.Length; i++)
            if (button.Data == skillsTreeSO.Datas[i] && i == testTreeSkillsLevel)
                return true;

        return false;
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    #region Debug
    [Space(10), Header("Debug")]
    [SerializeField] private Button addGlobalLevelButton;
    [SerializeField] private Button addSkillsTreeLevelButton;

    private void Awake()
    {
        addGlobalLevelButton.onClick.AddListener(OnAddGlobalLevelButtonClicked);
        addSkillsTreeLevelButton.onClick.AddListener(OnAddSkillsTreeLevelButtonClicked);
    }

    private void OnAddGlobalLevelButtonClicked()
    {
        testFGlobalPlayerLevel++;

        UpdatePanel();
    }

    private void OnAddSkillsTreeLevelButtonClicked()
    {
        testTreeSkillsLevel++;

        UpdatePanel();
    }
    #endregion
}