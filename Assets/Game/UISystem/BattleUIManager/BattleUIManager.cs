using Zenject;

public class BattleUIManager : AbstractUIManager
{
    [Inject]
    private readonly SharedObjects sharedObjects;

    [Inject]
    private readonly DiContainer diContainer;

    public BattleUpgradeStorage BattleUpgradeStorage { get; private set; }
    public BattleUpgradeHandler BattleUpgradeHandler { get; private set; }
    public BattleUpgradeConfigsPack BattleUpgradeConfigsPack =>
        sharedObjects.GetScriptableObject<BattleUpgradeConfigsPack>(
            Constants.BATTLE_UPGRADE_CONFIG_PACK
        );

    public override void Init()
    {
        BattleUpgradeStorage = new BattleUpgradeStorage(BattleUpgradeConfigsPack);
        BattleUpgradeHandler = new BattleUpgradeHandler(BattleUpgradeStorage);
        InitPanels();
        HideAllPanels();
        EventBus<ExperienceReachedEvnt>.Subscribe(OnExperienceReachedEvnt);
    }

    private void InitPanels()
    {
        foreach (var panel in Panels)
        {
            diContainer.Inject(panel);

            panel.Init(
                new object[]
                {
                    BattleUpgradeConfigsPack,
                    BattleUpgradeHandler,
                    BattleUpgradeStorage,
                }
            );
        }
    }

    private void OnExperienceReachedEvnt(ExperienceReachedEvnt evnt)
    {
        GetPanel(PanelType.BattleUpgrade).Show();
    }
}
