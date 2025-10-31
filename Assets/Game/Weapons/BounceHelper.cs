using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct BounceHelper
{
    private readonly AbstractWeapon weapon;
    private readonly int firstDamage;
    private readonly AbstractUnit firstTargetUnit;
    private readonly ObjectPoolSystem objectPoolSystem;
    private readonly int bounceCount;
    private const float BOUNCE_DELAY = 0.1f;
    private Vector3 pos;

    public BounceHelper(
        AbstractWeapon weapon,
        int firstDamage,
        AbstractUnit firstTargetUnit,
        ObjectPoolSystem objectPoolSystem,
        int bounceCount
    )
    {
        this.weapon = weapon;
        this.firstDamage = firstDamage;
        this.firstTargetUnit = firstTargetUnit;
        this.objectPoolSystem = objectPoolSystem;
        this.bounceCount = bounceCount;
        pos = firstTargetUnit.transform.position;
        StartBounces();
    }

    private async void StartBounces()
    {
        if (bounceCount == 0)
            return;

        for (int i = 0; i < bounceCount; i++)
        {
            await UniTask.WaitForSeconds(BOUNCE_DELAY);
            var radius = weapon.WeaponSOData.GetValueByTag<float>(Constants.BOUNCE_RADIUS);
            var target = StaticFunctions.OverlapSphere<AbstractUnit>(pos, radius, true).First();
            pos = target.transform.position;
            var coef = weapon.WeaponSOData.GetValueByTag<float>(Constants.BOUNCE_STRENGTH_DECREASE_PERCENT)/100f;
            var damage = firstDamage * coef * (i + 1);
            CreateBullet(pos, (int)damage);
        }
    }

	private void CreateBullet(Vector3 pos, int damage)
	{
		var bullet = objectPoolSystem.GetPoolableObject<Bullet>(weapon.WeaponSOData.BulletType.ToString());
		bullet.transform.position = pos;
        bullet.Init(weapon, objectPoolSystem, damage);
        bullet.SetActive();
	}
}
