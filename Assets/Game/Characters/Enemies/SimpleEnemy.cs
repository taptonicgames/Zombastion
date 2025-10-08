using System;
using UnityEngine;
using Zenject;

public class SimpleEnemy : AbstractEnemy
{
    [Inject] private readonly SceneReferences sceneReferences;

    private void Start()
    {
        abilitiesPair = new()
        {
            { AbilityType.Movement, new EnemyMovementAbility(this) },
        };

        unitActionsList = new() { new UnitIdleAction(this), new ZombieMoveAction(this) };

        foreach (var item in unitActionsList)
        {
            diContainer.Inject(item);
        }

        SetActionTypeForced(UnitActionType.Move);
    }

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == Constants.GATES_LAYER)
		{
			sceneReferences.castle.SetDamage(SOData.Damage);
            OnUnitDied();
		}
	}

	public override Type GetDamageRecieverType()
	{
        return typeof(SimpleEnemy);
	}
}
