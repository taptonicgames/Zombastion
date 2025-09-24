using UnityEngine;

public class AbstractPlayerUnit : AbstractUnit
{
    [SerializeField]
    protected PlayerSO SOData;

    public virtual int GetPlayerDamage()
    {
        var randomValue = Random.Range(0, 100);
        var damage = SOData.ShootDamage;

        if (randomValue <= SOData.CritProbability)
            damage *= SOData.CritDamage;

        return damage;
    }

    public virtual float GetPlayerShootDelay()
    {
        return SOData.ShootDelay;
    }
}
