using UnityEngine;

public class AbstractPlayerUnit : AbstractUnit
{
    [SerializeField]
    protected PlayerSO SOData;

	public override int Health
	{
		get => base.Health;
		protected set => health = Mathf.Clamp(health + value, 0, SOData.Health);
	}

	public override IGetAttackSOParameters GetAttackSOParameters()
	{
        return SOData;
    }

    public virtual int GetPlayerDamage()
    {
        var randomValue = Random.Range(0, 100);
        var damage = SOData.Damage;

        if (randomValue <= SOData.CritProbability)
            damage *= SOData.CritDamage;

        return damage;
    }
}
