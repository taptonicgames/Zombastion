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
    protected AbstractUnit shootingUnit;
    protected float angleToTarget;
    protected AbstractUnit targetUnit;
    protected Transform targetTr;
    public bool IsReady { get; protected set; } = true;

    public virtual void Fire(AbstractUnit shootingUnit, AbstractUnit targetUnit)
    {
        inFire = true;
        this.targetUnit = targetUnit;
        this.shootingUnit = shootingUnit;
    }

    public virtual void Fire(AbstractUnit shootingUnit, Transform targetTr)
    {
        inFire = true;
        this.targetTr = targetTr;
        this.shootingUnit = shootingUnit;
    }

    public virtual void StopFire()
    {
        inFire = false;
        targetUnit = null;
        targetTr = null;
        shootingUnit = null;
    }

    public WeaponSO WeaponSOData => weaponSO;

    public bool InFire
    {
        get => inFire;
    }
    public AbstractUnit TargetUnit => targetUnit;

    public AbstractUnit ShootingUnit
    {
        get => shootingUnit;
    }

    public int CalculateDamage()
    {
        var damage =
            shootingUnit is AbstractPlayerUnit
                ? WeaponSOData.ShootDamage * ((AbstractPlayerUnit)shootingUnit).GetPlayerDamage()
                : WeaponSOData.ShootDamage * shootingUnit.GetAttackSOParameters().Damage;

        return damage;
    }

    public float CalculateReloadTime()
    {
        var time = weaponSO.ShootDelay * shootingUnit.GetAttackSOParameters().ShootDelay;
        return time;
    }

	public virtual void SetAnimationPhase(int value) { }
}
