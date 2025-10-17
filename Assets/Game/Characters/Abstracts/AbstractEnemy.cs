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
    public int ExperienceForDestroy => SOData.ExperienceForDestroy;

    public override void Init()
    {
        base.Init();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
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
}
