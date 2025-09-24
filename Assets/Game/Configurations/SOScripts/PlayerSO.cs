using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSO", menuName = "ScriptableObjects/PlayerSO")]
public class PlayerSO : ScriptableObject
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

    public float Speed
    {
        get => speed;
    }
    public int ShootDamage
    {
        get => shootDamage;
    }
    public float ShootDelay
    {
        get => shootDelay;
    }
    public int Health
    {
        get => health;
    }
    public int CritDamage
    {
        get => critDamage;
    }
    public float CritProbability
    {
        get => critProbability;
    }
}
