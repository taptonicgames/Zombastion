using System.Collections.Generic;
using Zenject;

public class CardsUpgradeManager
{
    [Inject]
    private readonly SharedObjects sharedObjects;

    private BattleUpgradeConfigsPack BattleUpgradeConfigsPack =>
        sharedObjects.GetScriptableObject<BattleUpgradeConfigsPack>(
            Constants.BATTLE_UPGRADE_CONFIG_PACK
        );

    public IEnumerable<BattleUpgradeConfig> GetUpgradeConfigs()
    {
        var upgradeConfigs = BattleUpgradeConfigsPack.GetConfigs(BattleUpgradeType.TowerBuild);
        return upgradeConfigs;
    }
}
