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

    public IEnumerable<BattleUpgradeConfig> GetConfigs(BattleUpgradeType type)
    {
        var config = Configs.Where(a => a.UpgradeType == type);

        if (config == null)
            Debug.LogError($"{type} is not present in Configs");

        return config;
    }
}
