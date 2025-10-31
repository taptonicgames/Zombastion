using System.Collections;
using UnityEngine;
using Zenject;

public class CauldronBullet : Bullet
{
    [Inject]
    private readonly CoroutineManager coroutineManager;

    [SerializeField]
    private Transform puddle,
        bullet;
    private AbstractEnemy unit;

    public override void Init(
        AbstractWeapon weapon,
        ObjectPoolSystem objectPoolSystem,
        AbstractUnit targetUnit,
        int damage,
        bool isActive = true
    )
    {
        base.Init(weapon, objectPoolSystem, targetUnit, damage, isActive);
        SwitchBulletToPuddle(false);
    }

    public override void CompleteAction(int value)
    {
        SwitchBulletToPuddle(true);
        var damageRadius = weapon.WeaponSOData.GetValueByTag<float>(Constants.BULLET_DAMAGE_RADIUS);
        puddle.localScale = Vector3.one * damageRadius * 2f;
        var lifetime = weapon.WeaponSOData.GetValueByTag<float>(Constants.PUDDLE_LIFETIME);
        coroutineManager.InvokeActionDelay(Reset, lifetime);
    }

    private void SwitchBulletToPuddle(bool value)
    {
        bullet.gameObject.SetActive(!value);
        puddle.gameObject.SetActive(value);
    }

    protected override void Update() { }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != Constants.ENEMY_LAYER)
            return;

        var catchedUnit = other.GetComponent<AbstractEnemy>();

        if (catchedUnit == unit)
            return;

        var value = weapon.WeaponSOData.GetValueByTag<float>(
            Constants.ENEMY_SPEED_DECREASE_PERCENT
        );

        catchedUnit.AddDebuff(new ZombieSpeedDebuff(unit, value));
        unit = catchedUnit;
    }

    protected override IEnumerator DestroyDelay()
    {
        yield break;
    }
}
