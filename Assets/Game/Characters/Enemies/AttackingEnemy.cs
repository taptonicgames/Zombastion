using System;
using UnityEngine;

public class AttackingEnemy : AbstractEnemy
{
    [SerializeField] private AbstractWeapon zombieWeapon;

    private void Start()
    {
        weapon = zombieWeapon;
        abilitiesPair = new() { { AbilityType.Movement, new EnemyMovementAbility(this) } };

        unitActionsList = new()
        {
            new ZombieAttackAction(this),
            new ZombieMoveAction(this),
            new UnitIdleAction(this),
        };

        foreach (var item in unitActionsList)
        {
            diContainer.Inject(item);
        }

        SetActionTypeForced(UnitActionType.Move);
    }

	public override Type GetDamageRecieverType()
	{
		return typeof(AttackingEnemy);
	}
}
