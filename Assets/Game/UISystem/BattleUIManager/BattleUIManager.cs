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

    //public BattleUpgradeHandler BattleUpgradeHandler { get; private set; }
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
        EventBus<OpenPanelEvnt>.Subscribe(OnPanelOpenedEvnt);
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
        var panel = GetPanel(PanelType.BattleUpgrade);

        panel.Init(
            new object[]
            {
                BattleUpgradeConfigsPack,
                BattleUpgradeStorage,
                cardsUpgradeManager.GetUpgradeConfigs().ToArray(),
            }
        );

        panel.Show();
    }

    private void OnPanelOpenedEvnt(OpenPanelEvnt evnt)
    {
        var panel = GetPanel(evnt.type);

        if (!panel)
            return;

        panel.Init(null);
        panel.Show();
    }

    private void OnDestroy()
    {
        EventBus<ExperienceReachedEvnt>.Unsubscribe(OnExperienceReachedEvnt);
        EventBus<OpenPanelEvnt>.Unsubscribe(OnPanelOpenedEvnt);
    }
}