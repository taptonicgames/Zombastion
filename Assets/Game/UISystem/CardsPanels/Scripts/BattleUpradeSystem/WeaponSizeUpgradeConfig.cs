using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSizeUpgradeConfig", menuName = "Configs/Upgrades/WeaponSizeUpgradeConfig")]
public class WeaponSizeUpgradeConfig : BattleUpgradeConfig
{
    [field: Space(10)]
    [field: SerializeField]   public float DamageValue { get; private set; }
}