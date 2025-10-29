using System.Collections;
using System.Linq;
using UnityEngine;

public class CatapultBullet : Bullet
{
    public void CompleteAction(int obj)
    {
        var damageRadius = weapon.WeaponSOData.GetValueByTag<float>(Constants.BULLET_DAMAGE_RADIUS);

        var enemiesColliders = Physics
            .OverlapSphere(transform.position, damageRadius)
            .Where(a => a.GetComponent<AbstractEnemy>());

        foreach (var item in enemiesColliders)
        {
            var unit = item.GetComponent<AbstractUnit>();
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
