using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "ScriptableObjects/EnemySO")]
public class EnemySO : ScriptableObject, IGetAttackSOParameters
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private int shootDamage;

    [SerializeField]
    private float shootDelay;

    [SerializeField]
    private int health;

    public int ShootDamage => shootDamage;
    public float ShootDelay => shootDelay;
    public float Speed => speed;
    public int Health => health;
}
