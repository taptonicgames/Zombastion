using UnityEngine;

[CreateAssetMenu(fileName = "TowerBuildConfig", menuName = "Configs/Upgrades/TowerBuildConfig")]
public class TowerBuildConfig : BattleUpgradeConfig
{
	[field: SerializeField] public ParameterUpgradeType ParameterUpgradeType { get; private set; }
}