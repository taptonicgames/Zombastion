using System;
using System.Collections.Generic;
using UnityEngine;

// Скрипт отвечает за распределение апгрейдов после его выбора в панеле карточек
public class BattleUpgradeHandler
{
    private BattleUpgradeStorage _battleUpgradeStorage;

    //private Player _player;
    //private LevelTimer _levelTimer;
    //private LevelEnvironment _levelEnvironment;
    //private UIHandler _uIHandler;

    public event Action<BattleUpgradeConfig> UpgradeGeted;

    public BattleUpgradeHandler(BattleUpgradeStorage battleUpgradeStorage)
    //Player player,
    //LevelTimer levelTimer,
    //LevelEnvironment levelEnvironment,
    //UIHandler uIHandler)
    {
        _battleUpgradeStorage = battleUpgradeStorage;
        //_player = player;
        //_levelTimer = levelTimer;
        //_levelEnvironment = levelEnvironment;
        //_uIHandler = uIHandler;
    }

    public void HandleUpgrades(List<BattleUpgradeConfig> upgrades)
    {
        foreach (BattleUpgradeConfig upgrade in upgrades)
        {
            var action = GetAction(upgrade);
            action(upgrade);
            UpgradeGeted?.Invoke(upgrade);
        }

        //if (upgrades.Count > 0)
        //    PoolManager.GetPool(_player.BuffEffectPrefab, _player.transform.position);

        _battleUpgradeStorage.OnUpgradeGeted(upgrades);
        _battleUpgradeStorage.ResetList();
    }

    private Action<BattleUpgradeConfig> GetAction(BattleUpgradeConfig upgrade)
    {
        return upgrade.UpgradeType switch
        {
            BattleUpgradeType.TowerBuild => SetSpeedUpgrade,

            _ => Exception,
        };
    }

    private void SetSpeedUpgrade(BattleUpgradeConfig upgrade)
    {
        //_player.AddBonusSpeed(upgrade.Value);
    }

    private void SetDamageUpgrade(BattleUpgradeConfig upgrade)
    {
        //_player.AddBonusDamage(upgrade.Value);
    }

    private void SetWeaponSizeUpgrade(BattleUpgradeConfig upgrade)
    {
        //var config = upgrade as WeaponSizeUpgradeConfig;
        //_player.AddBonusSize(config.Value, config.DamageValue);
    }

    private void SetAddTimeUpgrade(BattleUpgradeConfig upgrade)
    {
        //_levelTimer.AddTime(upgrade.Value);
    }

    private void SetMeteoriteRain(BattleUpgradeConfig upgrade)
    {
        //_levelEnvironment.ActivateMeteoriteRain(upgrade.Value);
    }

    private void SetFlyingSpheres(BattleUpgradeConfig upgrade)
    {
        //_player.AddFlyingSpheres(Mathf.RoundToInt(upgrade.Value));
    }

    private void SetBonusRewardUpgrade(BattleUpgradeConfig upgrade)
    {
        //_uIHandler.AddBonusPercent(upgrade.Value);
    }

    private void SetSlowDawnEnemies(BattleUpgradeConfig upgrade)
    {
        //_levelEnvironment.SetSlowDownEnemies(upgrade.Value);
    }

    private void SetShieldUpgrade(BattleUpgradeConfig upgrade)
    {
        //_player.AddShield(upgrade.Value);
    }

    private void SetDoubleWeaponUpgrade(BattleUpgradeConfig upgrade)
    {
        //_player.DoublePistols(upgrade.Value);
    }

    private void SetRicochetUpgrade(BattleUpgradeConfig upgrade)
    {
        //_player.AddRicochetCount(Mathf.RoundToInt(upgrade.Value));
    }

    private void SetGrenadesUpgrade(BattleUpgradeConfig upgrade)
    {
        //_player.ActivateGrenades(upgrade.Value);
    }

    private void SetLightningStrikes(BattleUpgradeConfig upgrade)
    {
        //_levelEnvironment.ActivateLightningStrikes(upgrade.Value);
    }

    private void SetAddChainsaw(BattleUpgradeConfig upgrade)
    {
        //_player.AddChainsaw(upgrade.Value);
    }

    private void SetAxesUpgrade(BattleUpgradeConfig upgrade)
    {
        //_player.ActivateAxes(upgrade.Value);
    }

    private void SetHealPlayer(BattleUpgradeConfig upgrade)
    {
        //_player.Heal(upgrade.Value);
    }

    private void SetIncreaseBag(BattleUpgradeConfig upgrade)
    {
        //_player.IncreaseBag(upgrade.Value);
    }

    private void Exception(BattleUpgradeConfig upgrade)
    {
        throw new NotImplementedException($"{upgrade} not implemented");
    }
}
