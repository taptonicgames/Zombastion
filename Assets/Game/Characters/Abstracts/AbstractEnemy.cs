using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class AbstractEnemy : AbstractUnit
{
    [Inject]
    private readonly SceneReferences sceneReferences;

    public override void Init()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        abilitiesPair.Add(AbilityType.Movement, new EnemyMovementAbility(this));
    }
}
