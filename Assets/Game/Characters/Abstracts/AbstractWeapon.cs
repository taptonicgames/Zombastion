using UnityEngine;
using Zenject;

public abstract class AbstractWeapon : MonoBehaviour
{
    [Inject]
    protected ObjectPoolSystem objectPoolSystem;

    [SerializeField]
    private WeaponSO weaponSO;
    protected bool inFire, inReload;
    protected AbstractUnit targetUnit;
    protected float angleToTarget;

    public virtual void Fire(AbstractUnit targetUnit)
    {
        inFire = true;
        this.targetUnit = targetUnit;
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
            angleToTarget = StaticFunctions.ObjectFinishTurning(transform, targetUnit.transform.position);
        }
    }
}
