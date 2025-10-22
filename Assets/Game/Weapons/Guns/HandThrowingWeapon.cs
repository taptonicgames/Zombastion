using System;
using UnityEngine;

public class HandThrowingWeapon : AbstractWeapon
{
	[SerializeField] private Transform throwingBoneTr;

	private void Start()
	{
		throwingBoneTr.gameObject.SetActive(false);
	}

	public override void Fire(AbstractUnit shootingUnit, AbstractUnit targetUnit)
	{
		base.Fire(shootingUnit, targetUnit);
		InstantiateBullet();
	}

	public override void Fire(AbstractUnit shootingUnit, Transform targetTr)
	{
		base.Fire(shootingUnit, targetTr);
		InstantiateBullet();
	}

	private void InstantiateBullet()
	{
		var bullet = objectPoolSystem.GetPoolableObject<BoneBullet>(
				WeaponSOData.BulletType.ToString()
			);

		bullet.transform.position = throwingBoneTr.position;
		StaticFunctions.ObjectFinishTurning(bullet.transform, GetTargetPos(), -360, 360);
		bullet.Init(this, objectPoolSystem, CalculateDamage());
	}

	private Vector3 GetTargetPos()
	{
		if (targetUnit)
			return targetUnit.transform.position + Vector3.up;
		else
			return targetTr.position;
	}
}
