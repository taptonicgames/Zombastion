using System;
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private AbstractWeapon weapon;
    private WeaponSO SOData;
    private ObjectPoolSystem objectPoolSystem;
	private int damage;
	private bool isActive;

    public void Init(AbstractWeapon weapon, ObjectPoolSystem objectPoolSystem, int damage)
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
        isActive = false;
        objectPoolSystem.ReleasePoolableObject(SOData.BulletType.ToString(), gameObject);
        StopAllCoroutines();
    }

    private void Update()
    {
        if (isActive)
            transform.Translate(Vector3.forward * Time.deltaTime * SOData.BulletSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        var unit = other.GetComponentInParent<AbstractUnit>();

        if (unit != null)
        {
            unit.SetDamage(damage);
        }

        Reset();
    }
}
