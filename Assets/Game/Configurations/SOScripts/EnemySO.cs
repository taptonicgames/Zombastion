using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "ScriptableObjects/EnemySO")]
public class EnemySO : ScriptableObject, IGetAttackSOParameters
{
    [field: SerializeField]
    public int Damage { get; private set; }

    [field: SerializeField]
    public float ShootDelay { get; private set; }

    [field: SerializeField]
    public float Speed { get; private set; }

    [field: SerializeField]
    public int Health { get; private set; }
}
