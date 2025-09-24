using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "ScriptableObjects/WeaponSO")]
public class WeaponSO : ScriptableObject
{
    [SerializeField]
    private float shootDelay;

    [SerializeField]
    private int shootDamage;

    [SerializeField]
    private BulletType bulletType;

    [SerializeField]
    private float bulletSpeed;

    [SerializeField]
    private float attackDistance;

    public float ShootDelay => shootDelay;
    public int ShootDamage => shootDamage;
    public BulletType BulletType => bulletType;
    public float BulletSpeed => bulletSpeed;
    public float AttackDistance => attackDistance;
}
