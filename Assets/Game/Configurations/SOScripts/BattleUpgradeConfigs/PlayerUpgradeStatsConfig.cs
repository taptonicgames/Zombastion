using UnityEngine;

[CreateAssetMenu(fileName = "PlayerUpgradeStatsConfig", menuName = "Configs/Upgrades/PlayerUpgradeStatsConfig")]
public class PlayerUpgradeStatsConfig : BattleUpgradeConfig
{
	[field: SerializeField] public ParameterUpgradeType ParameterUpgradeType {  get; private set; }
}