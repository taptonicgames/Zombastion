using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;
using Rnd = UnityEngine.Random;

public class CardsUpgradeManager
{
    [Inject]
    private readonly SharedObjects sharedObjects;

    [Inject]
    private readonly AbstractSavingManager savingManager;

    private BattleUpgradeConfigsPack BattleUpgradeConfigsPack =>
        sharedObjects.GetScriptableObject<BattleUpgradeConfigsPack>(
            Constants.BATTLE_UPGRADE_CONFIG_PACK
        );

    private UpgradesSavingData UpgradesSavingData =>
        savingManager.GetSavingData<UpgradesSavingData>(SavingDataType.Upgrades);

    public IEnumerable<BattleUpgradeConfig> GetUpgradeConfigs(GetUpgradeConfigDTO dto)
    {
        return BattleUpgradeConfigsPack.GetConfigs(dto);
    }

    public IEnumerable<BattleUpgradeConfig> GetAvailableUpgradeConfigs()
    {
        List<BattleUpgradeConfig> configs = new();
        GetPlayersConfigs(configs);
        GetTowersConfigs(configs);
        configs = RandomizeConfigs(configs, 3);
        SaveChoosenConfigs(configs);
        return configs;
    }

    private void GetPlayersConfigs(List<BattleUpgradeConfig> configs)
    {
        var playersConfigs = GetUpgradeConfigs(new GetUpgradeConfigDTO(CharacterType.Player));
        configs.AddRange(playersConfigs);
    }

    private void GetTowersConfigs(List<BattleUpgradeConfig> configs)
    {
        var towers = new[]
        {
            WeaponType.Catapult,
            WeaponType.Crossbow,
            WeaponType.Cauldron,
            WeaponType.Coil,
        };

        var towersExist = UpgradesSavingData.GetUpgradeConfigDTOs(CharacterType.Tower);

        foreach (var towerWeaponType in towers)
        {
            IEnumerable<BattleUpgradeConfig> towerConfigs = default;

            if (towersExist.Any(a => a.weaponType == towerWeaponType))
            {
                towerConfigs = GetUpgradeConfigs(
                    new GetUpgradeConfigDTO(
                        CharacterType.Tower,
                        towerWeaponType,
                        BattleUpgradeType.Stats
                    )
                );
            }
            else
            {
                towerConfigs = GetUpgradeConfigs(
                    new GetUpgradeConfigDTO(
                        CharacterType.Tower,
                        towerWeaponType,
                        BattleUpgradeType.TowerBuild
                    )
                );
            }

            configs.AddRange(towerConfigs);
        }
    }

    private List<BattleUpgradeConfig> RandomizeConfigs(
        List<BattleUpgradeConfig> configs,
        int amount
    )
    {
        List<BattleUpgradeConfig> list = new();

        for (int i = 0; i < amount; i++)
        {
            var index = Rnd.Range(0, configs.Count);
            list.Add(configs[index]);
            configs.Remove(configs[index]);
        }

        return list;
    }

    private void SaveChoosenConfigs(List<BattleUpgradeConfig> configs)
    {
        foreach (var config in configs)
        {
            GetUpgradeConfigDTO dto = new(
                config.CharacterType,
                config.WeaponType,
                config.UpgradeType,
                config.GetParameterType<ParameterUpgradeType>(),
                config.RareType
            );

            UpgradesSavingData.AddUpgradeConfigDTO(dto);
        }
    }
}
