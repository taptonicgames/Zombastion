using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected AbstractWeapon weapon;
    protected WeaponSO SOData;
    protected ObjectPoolSystem objectPoolSystem;
    protected int damage;
    protected AbstractUnit targetUnit;

	public bool IsActive { get; private set; }

    public virtual void Init(
        AbstractWeapon weapon,
        ObjectPoolSystem objectPoolSystem,
        AbstractUnit targetUnit,
        int damage,
        bool isActive = true
    )
    {
        this.weapon = weapon;
        SOData = weapon.WeaponSOData;
        this.objectPoolSystem = objectPoolSystem;
        this.damage = damage;
        this.targetUnit = targetUnit;
        StartCoroutine(DestroyDelay());
        IsActive = isActive;
    }

    protected virtual IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(5);
        Reset();
    }

    protected virtual void Reset()
    {
        if (!IsActive)
            return;
        IsActive = false;
        objectPoolSystem.ReleasePoolableObject(SOData.BulletType.ToString(), gameObject);
        StopAllCoroutines();
    }

    protected virtual void Update()
    {
        if (IsActive)
            transform.Translate(Vector3.forward * Time.deltaTime * SOData.BulletSpeed);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        FindRecieverAndDamage(other);
    }

    private void OnCollisionEnter(Collision collision)
    {
        FindRecieverAndDamage(collision.collider);
    }

    private void FindRecieverAndDamage(Collider collider)
    {
        var damageReciever = collider.GetComponentInParent<IDamageReciever>();

        if (damageReciever != null)
        {
            if (damageReciever is AbstractUnit)
            {
                if (targetUnit)
                {
                    if (damageReciever.GetDamageRecieverType() == targetUnit.GetType())
                        damageReciever.SetDamage(damage);
                    else
                        return;
                }
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

    public virtual void SetActive() { IsActive = true; }
    public virtual void CompleteAction(int value) { }
}
