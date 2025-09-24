using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSO", menuName = "ScriptableObjects/PlayerSO")]
public class PlayerSO : ScriptableObject, IGetAttackSOParameters
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private int shootDamage;

    [SerializeField]
    private float shootDelay;

    [SerializeField]
    private int health;

    [SerializeField]
    private int critDamage;

    [SerializeField]
    private float critProbability;

    public float Speed => speed;
    public int ShootDamage => shootDamage;
    public float ShootDelay => shootDelay;
    public int Health => health;
    public int CritDamage => critDamage;
    public float CritProbability => critProbability;
}
