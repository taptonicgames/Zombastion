using System;
using UnityEngine;

public class ZombieAxe : AbstractEnemy
{
	[SerializeField]
	private AbstractWeapon zombieWeapon;

	private void Start()
	{
		weapon = zombieWeapon;
		abilitiesPair = new() { { AbilityType.Movement, new EnemyMovementAbility(this) } };

		unitActionsList = new()
		{
			new ZombieAxeAttackAction(this),
			new ZombieMoveAction(this),
			new UnitIdleAction(this),
			new UnitPauseAction(this),
		};

		foreach (var item in unitActionsList)
		{
			diContainer.Inject(item);
		}

		SetActionTypeForced(UnitActionType.Move);
	}

	public override Type GetDamageRecieverType()
	{
		return typeof(ZombieAxe);
	}
}
