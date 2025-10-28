using System.Collections;
using UnityEngine;

public class CatapultBullet : Bullet
{
    public void CompleteAction(int obj)
    {
        weapon.TargetUnit.SetDamage(damage);
        Reset();
    }

    protected override void Update() { }

    protected override void OnTriggerEnter(Collider other) { }
	protected override IEnumerator DestroyDelay()
	{
        yield break;
	}
}
