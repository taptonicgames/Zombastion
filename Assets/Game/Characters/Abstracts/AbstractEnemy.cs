using System.Collections.Generic;
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

        abilitiesPair = new()
        {
            { AbilityType.Movement, new EnemyMovementAbility(this) },
            { AbilityType.Attack, new UnitAttackAbility(this, new() { CharacterType.Player }) },
        };
    }

    public override void OnUnitDied()
    {
        base.OnUnitDied();
        enemyManager.RemoveEnemyFromList(this);
        Destroy(gameObject);
    }
}
