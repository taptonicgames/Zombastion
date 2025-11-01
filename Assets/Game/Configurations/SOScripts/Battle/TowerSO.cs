using UnityEngine;

[CreateAssetMenu(fileName = "TowerSO", menuName = "ScriptableObjects/TowerSO")]
public class TowerSO : PlayerSO
{
    [field: Space(10), Header("Modifiers")]
    [field: SerializeField] public string Id { get; private set; }
    [field: SerializeField] public CharacterType CharacterType { get; private set; }
    [field: SerializeField] public WeaponType WeaponType { get; private set; }
    [field: SerializeField] public int UnlockLevel { get; private set; }
    [field: SerializeField] public float SpeedCoefficient { get; private set; }
    [field: SerializeField] public float ShootDamageCoefficient { get; private set; }
    [field: SerializeField] public float ShootDelayCoefficient { get; private set; }
    [field: SerializeField] public float HealthCoefficient { get; private set; }
    [field: SerializeField] public float CritDamageCoefficient { get; private set; }
    [field: SerializeField] public float CritProbabilityCoefficient { get; private set; }
    [field: SerializeField] public TowerUIData UIData { get; private set; }

    private void OnValidate()
    {
        if (Id != $"{WeaponType}{CharacterType}")
            Id = $"{WeaponType}{CharacterType}";
    }
}