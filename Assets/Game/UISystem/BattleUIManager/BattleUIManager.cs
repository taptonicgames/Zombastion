using System;
using UnityEngine;
using Zenject;

public class BattleUIManager : AbstractUIManager
{
    [Inject]
    private readonly SharedObjects sharedObjects;

    [Inject]
    private readonly DiContainer diContainer;

    [Inject]
    private readonly SceneReferences sceneReferences;

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
        EventBus<ExperienceReachedEvnt>.Subscribe(OnExperienceReachedEvnt);
    }

    private void InitPanels()
    {
        foreach (var item in panelPrefabs)
        {
            var panel = diContainer.InstantiatePrefabForComponent<AbstractPanel>(item, transform);
            Panels.Add(panel);
        }
    }

    private void OnExperienceReachedEvnt(ExperienceReachedEvnt evnt)
    {
        var panel = GetPanel(PanelType.BattleUpgrade);

        BattleUpgradeConfig[] upgradeConfigs = new BattleUpgradeConfig[]
        {
            BattleUpgradeConfigsPack.GetConfig(BattleUpgradeType.TowerBuild),
        };

        panel.Init(new object[] { BattleUpgradeConfigsPack, BattleUpgradeStorage, upgradeConfigs });
        panel.Show();
    }
}
