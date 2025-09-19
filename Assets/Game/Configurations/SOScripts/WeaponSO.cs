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

    public float ShootDelay
    {
        get => shootDelay;
    }

    public int ShootDamage
    {
        get => shootDamage;
    }

    public BulletType BulletType
    {
        get => bulletType;
    }

    public float BulletSpeed
    {
        get => bulletSpeed;
    }
}
