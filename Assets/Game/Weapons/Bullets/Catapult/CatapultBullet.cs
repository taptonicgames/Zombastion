using System.Collections;
using UnityEngine;

public class CatapultBullet : Bullet
{
    public override void CompleteAction(int value)
    {
        var damageRadius = weapon.WeaponSOData.GetValueByTag<float>(Constants.BULLET_DAMAGE_RADIUS);
        var enemies = StaticFunctions.OverlapSphere<AbstractUnit>(transform.position, damageRadius);

        foreach (var unit in enemies)
        {
            unit.SetDamage(damage);
        }

        Reset();
    }

    protected override void Update() { }

    protected override void OnTriggerEnter(Collider other) { }

    protected override IEnumerator DestroyDelay()
    {
        yield break;
    }
}
