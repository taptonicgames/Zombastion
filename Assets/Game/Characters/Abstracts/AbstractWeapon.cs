using UnityEngine;
using Zenject;

public abstract class AbstractWeapon : MonoBehaviour
{
    [Inject]
    protected ObjectPoolSystem objectPoolSystem;

    [SerializeField]
    private WeaponSO weaponSO;
    protected bool inFire,
        inReload;
    protected AbstractUnit shootingUnit,
        targetUnit;
    protected float angleToTarget;

    public virtual void Fire(AbstractUnit shootingUnit, AbstractUnit targetUnit)
    {
        inFire = true;
        this.targetUnit = targetUnit;
        this.shootingUnit = shootingUnit;
    }

    public virtual void StopFire()
    {
        inFire = false;
    }

    public WeaponSO WeaponSOData => weaponSO;

    public bool InFire
    {
        get => inFire;
    }

    private void Update()
    {
        if (targetUnit)
        {
            angleToTarget = StaticFunctions.ObjectFinishTurning(
                transform,
                targetUnit.transform.position
            );
        }
    }

    public int CalculateDamage()
    {
        var damage =
            shootingUnit is AbstractPlayerUnit
                ? WeaponSOData.ShootDamage * ((AbstractPlayerUnit)shootingUnit).GetPlayerDamage()
                : WeaponSOData.ShootDamage * shootingUnit.GetAttackSOParameters().ShootDamage;

        return damage;
    }

    public float CalculateReloadTime()
    {
        var time = weaponSO.ShootDelay * shootingUnit.GetAttackSOParameters().ShootDelay;
        return time;
    }
}
