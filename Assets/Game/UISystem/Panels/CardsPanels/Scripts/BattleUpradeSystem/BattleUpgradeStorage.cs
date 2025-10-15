using System.Collections.Generic;
using UnityEngine;

public class BattleUpgradeStorage
{
    private readonly BattleUpgradeConfigsPack _upgradesPack;

    private List<BattleUpgradeConfig> _configs = new List<BattleUpgradeConfig>();

    public Dictionary<BattleUpgradeType, int> Upgrades = new Dictionary<BattleUpgradeType, int>()
    {
        {BattleUpgradeType.TowerBuild, 0},
    };

    public BattleUpgradeStorage(BattleUpgradeConfigsPack upgradesPack)
    {
        _upgradesPack = upgradesPack;
    }

    public BattleUpgradeConfig GetBattleUpgrade()
    {
        var type = GetUpgradeType();

        BattleUpgradeConfig config = GetRandomUpgrade(type);

        _configs.Remove(config);

        return config;
    }

    public void ResetList()
    {
        _configs.Clear();

        _configs.AddRange(_upgradesPack.Configs);
    }

    private BattleUpgradeType GetUpgradeType()
    {
        List<BattleUpgradeType> types = new List<BattleUpgradeType>();

        for (int i = 0; i < _configs.Count; i++)
        {
            bool isAvailable = false;
            isAvailable = CheckEmptyType(types, _configs[i]);

            if (isAvailable)
                types.Add(_configs[i].UpgradeType);
        }

        int random = Random.Range(0, types.Count);
        return types[random];
    }

    private bool CheckEmptyType(List<BattleUpgradeType> types, BattleUpgradeConfig upgradeConfig)
    {
        if (upgradeConfig.IsAccumulating)
        {
            var value = Upgrades[upgradeConfig.UpgradeType];
            if (value >= upgradeConfig.MaxPickedCount)
                return false;
        }

        foreach (var type in types)
        {
            if (type == upgradeConfig.UpgradeType)
                return false;
        }

        return true;
    }

    private BattleUpgradeConfig GetRandomUpgrade(BattleUpgradeType type)
    {
        List<BattleUpgradeConfig> upgrades = new List<BattleUpgradeConfig>();
        List<float> percentages = new List<float>();

        for (int i = 0; i < _configs.Count; i++)
        {
            if (_configs[i].UpgradeType == type)
            {
                upgrades.Add(_configs[i]);
                percentages.Add(_configs[i].ChanceShowingPercentage);
            }
        }

        int index = PercentageCalculator.CalculateVariant(percentages.ToArray());

        return upgrades[index];
    }

    public void OnUpgradeGeted(List<BattleUpgradeConfig> upgrades)
    {

        foreach (BattleUpgradeConfig upgrade in upgrades)
        {
            var value = Upgrades[upgrade.UpgradeType];
            value++;
            Upgrades[upgrade.UpgradeType] = value;
        }
    }
}