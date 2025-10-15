using UnityEngine;
using Zenject;
using Rnd = UnityEngine.Random;

public abstract class AbstractPlayerUnit : AbstractUnit
{
    [SerializeField]
    protected PlayerSO SOData;
    [Inject]
    protected readonly DiContainer diContainer;
	[Inject]
    protected readonly PlayerCharacterModel playerCharacterModel;

    public override int Health
    {
        get => base.Health;
        set => health = Mathf.Clamp(value, 0, SOData.Health);
    }

    public override IGetAttackSOParameters GetAttackSOParameters()
    {
        return SOData;
    }

    public virtual int GetPlayerDamage()
    {
        var randomValue = Rnd.Range(0, 100);
        var damage = SOData.Damage;

        if (randomValue <= SOData.CritProbability)
            damage *= SOData.CritDamage;

        return damage;
    }

    public virtual int HealthResurectionOnBase => SOData.HealthResurectionPerSecond;
}
