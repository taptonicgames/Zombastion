using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class AbstractEnemy : AbstractUnit
{
    [Inject]
    protected readonly DiContainer diContainer;

    [Inject]
    protected readonly EnemyManager enemyManager;

    public override void Init()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        abilitiesPair.Add(AbilityType.Movement, new EnemyMovementAbility(this));
    }

    public override void OnUnitDied()
    {
        base.OnUnitDied();
        enemyManager.RemoveEnemyFromList(this);
        Destroy(gameObject);
    }
}
