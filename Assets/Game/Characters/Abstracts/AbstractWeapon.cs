using UnityEngine;
using Zenject;

public abstract class AbstractWeapon : MonoBehaviour
{
    [Inject] protected ObjectPoolSystem objectPoolSystem;
    [SerializeField] private WeaponSO weaponSO;
    protected bool inFire;

    public virtual void Fire()
    {
        inFire = true;
    }

    public virtual void StopFire()
    {
        inFire = false;
    }

    public WeaponSO WeaponSOData => weaponSO;
}
