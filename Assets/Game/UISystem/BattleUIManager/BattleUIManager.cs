using System;
using System.Linq;
using UnityEngine;
using Zenject;

public class BattleUIManager : AbstractUIManager
{
    [Inject]
    private readonly SharedObjects sharedObjects;

    [Inject]
    private readonly DiContainer diContainer;

    [Inject]
    private readonly CardsUpgradeManager cardsUpgradeManager;

    [SerializeField]
    private GameObject[] panelPrefabs;

    public BattleUpgradeStorage BattleUpgradeStorage { get; private set; }

    public BattleUpgradeConfigsPack BattleUpgradeConfigsPack =>
        sharedObjects.GetScriptableObject<BattleUpgradeConfigsPack>(
            Constants.BATTLE_UPGRADE_CONFIG_PACK
        );

    public override void Init()
    {
        BattleUpgradeStorage = new BattleUpgradeStorage(BattleUpgradeConfigsPack);
        InitPanels();
        HideAllPanels();
        ShowGamePanels();
        EventBus<ExperienceReachedEvnt>.Subscribe(OnExperienceReachedEvnt);
        EventBus<RoundCompleteEvnt>.Subscribe(OnRoundCompleteEvnt);
    }

    private void InitPanels()
    {
        foreach (var item in panelPrefabs)
        {
            var panel = diContainer.InstantiatePrefabForComponent<AbstractPanel>(item, transform);
            Panels.Add(panel);
            panel.Init();
        }
    }

    private void ShowGamePanels()
    {
        GetPanel(PanelType.PlayerExperience).Show();
    }

    private void OnExperienceReachedEvnt(ExperienceReachedEvnt evnt)
    {
        var initObjects = new object[]
        {
            BattleUpgradeConfigsPack,
            BattleUpgradeStorage,
            cardsUpgradeManager.GetUpgradeConfigs().ToArray(),
        };

        OpenPanel(PanelType.BattleUpgrade, initObjects);
    }

    private void OpenPanel(PanelType type, object[] initObjects = null)
    {
        var panel = GetPanel(type);

        if (!panel)
            return;

        panel.Init(initObjects);
        panel.Show();
    }

    private void OnRoundCompleteEvnt(RoundCompleteEvnt evnt)
    {
        switch (evnt.type)
        {
            case RoundCompleteType.Fail:
                OpenPanel(PanelType.LoseRound);
                break;
            case RoundCompleteType.Win:
                OpenPanel(PanelType.WinRound);
                break;
            default:
                break;
        }
    }
}
