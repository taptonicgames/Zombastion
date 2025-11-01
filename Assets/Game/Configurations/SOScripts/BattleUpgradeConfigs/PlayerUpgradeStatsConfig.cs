using UnityEngine;

[CreateAssetMenu(
    fileName = "PlayerUpgradeStatsConfig",
    menuName = "Configs/Upgrades/PlayerUpgradeStatsConfig"
)]
public class PlayerUpgradeStatsConfig : BattleUpgradeConfig
{
    [field: SerializeField]
    public ParameterUpgradeType ParameterUpgradeType { get; private set; }

    public override T GetParameterType<T>()
    {
        if (typeof(T) == typeof(ParameterUpgradeType))
            return (T)(object)ParameterUpgradeType;
        return base.GetParameterType<T>();
    }
}
