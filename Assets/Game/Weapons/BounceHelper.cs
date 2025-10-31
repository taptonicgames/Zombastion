using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public struct BounceHelper
{
    private readonly AbstractWeapon weapon;
    private readonly int firstDamage;
    private AbstractUnit targetUnit;
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
        targetUnit = firstTargetUnit;
        this.objectPoolSystem = objectPoolSystem;
        this.bounceCount = bounceCount;
        pos = Vector3.zero;
        SetPosition(firstTargetUnit);
        StartBounces();
    }

    private void SetPosition(AbstractUnit firstTargetUnit)
    {
        pos = firstTargetUnit.transform.position + Vector3.up;
    }

    private async void StartBounces()
    {
        if (bounceCount == 0)
            return;

        for (int i = 0; i < bounceCount; i++)
        {
            await UniTask.WaitForSeconds(BOUNCE_DELAY);
            var radius = weapon.WeaponSOData.GetValueByTag<float>(Constants.BOUNCE_RADIUS);
            var currentTargetUnit = targetUnit;

            targetUnit = StaticFunctions
                .OverlapSphere<AbstractUnit>(pos, radius, true)
                .Where(a => a.IsEnable && a != currentTargetUnit)
                .FirstOrDefault();

            if (!targetUnit)
                return;

            var coef =
                weapon.WeaponSOData.GetValueByTag<float>(Constants.BOUNCE_STRENGTH_DECREASE_PERCENT)
                / 100f;

            var damage = firstDamage - firstDamage * (coef * (i + 1));
            CreateBullet(pos, (int)damage);
            SetPosition(targetUnit);
        }
    }

    private void CreateBullet(Vector3 pos, int damage)
    {
        var bullet = objectPoolSystem.GetPoolableObject<Bullet>(
            weapon.WeaponSOData.BulletType.ToString()
        );
        bullet.transform.position = pos;
        bullet.Init(weapon, objectPoolSystem, targetUnit, damage);
        bullet.SetActive();
    }
}
