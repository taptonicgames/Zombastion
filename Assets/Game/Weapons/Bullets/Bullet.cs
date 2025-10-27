using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected AbstractWeapon weapon;
    protected WeaponSO SOData;
    protected ObjectPoolSystem objectPoolSystem;
    protected int damage;
    protected bool isActive;

    public virtual void Init(AbstractWeapon weapon, ObjectPoolSystem objectPoolSystem, int damage)
    {
        this.weapon = weapon;
        SOData = weapon.WeaponSOData;
        this.objectPoolSystem = objectPoolSystem;
        this.damage = damage;
        StartCoroutine(DestroyDelay());
        isActive = true;
    }

    private IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(5);
        Reset();
    }

    private void Reset()
    {
        if (!isActive)
            return;
        isActive = false;
        objectPoolSystem.ReleasePoolableObject(SOData.BulletType.ToString(), gameObject);
        StopAllCoroutines();
    }

    protected virtual void Update()
    {
        if (isActive)
            transform.Translate(Vector3.forward * Time.deltaTime * SOData.BulletSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        var damageReciever = other.GetComponentInParent<IDamageReciever>();

        if (damageReciever != null)
        {
            if (damageReciever is AbstractUnit)
            {
                if (
                    weapon.TargetUnit
                    && damageReciever.GetDamageRecieverType() == weapon.TargetUnit.GetType()
                )
                    damageReciever.SetDamage(damage);
            }
            else
                damageReciever.SetDamage(damage);

            if (
                damageReciever is AbstractUnit
                && (AbstractUnit)damageReciever == weapon.ShootingUnit
            )
                return;
        }

        Reset();
    }
}
