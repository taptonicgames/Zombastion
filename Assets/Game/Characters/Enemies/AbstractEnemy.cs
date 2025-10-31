using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public abstract class AbstractEnemy : AbstractUnit
{
    [Inject]
    protected readonly DiContainer diContainer;

    [Inject]
    protected readonly EnemyManager enemyManager;

    [SerializeField]
    protected EnemySO SOData;

    protected List<AbstractDebuff> debuffs = new();
    public int ExperienceForDestroy => SOData.ExperienceForDestroy;

    public override void Init()
    {
        base.Init();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.speed = SOData.Speed;
    }

    public override IGetAttackSOParameters GetAttackSOParameters()
    {
        return SOData;
    }

    public override void OnUnitDied()
    {
        base.OnUnitDied();
        enemyManager.RemoveEnemyFromList(this);
        Destroy(gameObject, 0.1f);
    }

    public void AddDebuff(AbstractDebuff debuff)
    {
        debuffs.Add(debuff);
    }
}
