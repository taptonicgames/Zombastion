using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(
    fileName = "BattleUpgradeConfigsPack",
    menuName = "Configs/Upgrades/BattleUpgradeConfigsPack"
)]
public class BattleUpgradeConfigsPack : ScriptableObject
{
    [field: SerializeField]
    public BattleUpgradeConfig[] Configs { get; private set; }

    [field: Space(10)]
    [field: Header("Common gradients")]
    [field: SerializeField]
    public Color CommonGradientStartColor { get; private set; } = Color.white;

    [field: SerializeField]
    public Color CommonGradientEndColor { get; private set; } = Color.white;

    [field: Space(10)]
    [field: Header("Rare gradients")]
    [field: SerializeField]
    public Color RareGradientStartColor { get; private set; } = Color.white;

    [field: SerializeField]
    public Color RareGradientEndColor { get; private set; } = Color.white;

    [field: Space(10)]
    [field: Header("Epic gradients")]
    [field: SerializeField]
    public Color EpicGradientStartColor { get; private set; } = Color.white;

    [field: SerializeField]
    public Color EpicGradientEndColor { get; private set; } = Color.white;

    [field: Space(10)]
    [field: Header("Legend gradients")]
    [field: SerializeField]
    public Color LegendGradientStartColor { get; private set; } = Color.white;

    [field: SerializeField]
    public Color LegendGradientEndColor { get; private set; } = Color.white;

    public IEnumerable<BattleUpgradeConfig> GetConfigs(GetUpgradeConfigDTO dto)
    {
		var upgradeConfigs = Configs.Where(a =>
			a.CharacterType == dto.characterType
		);

		if (dto.weaponType != WeaponType.None)
		{
			upgradeConfigs = upgradeConfigs.Where(a => a.WeaponType == dto.weaponType);
		}

		if (dto.upgradeType != BattleUpgradeType.None)
		{
			upgradeConfigs = upgradeConfigs.Where(a => a.UpgradeType == dto.upgradeType);
		}

		if (dto.parameterUpgradeType != ParameterUpgradeType.None)
		{
			upgradeConfigs = upgradeConfigs.Where(a =>
				a.GetParameterType<ParameterUpgradeType>() == dto.parameterUpgradeType
			);
		}

		return upgradeConfigs;
	}
}
