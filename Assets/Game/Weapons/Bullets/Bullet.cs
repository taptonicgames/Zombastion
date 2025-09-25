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
        if (!isActive)
            return;
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

        if (unit != null && unit.GetType() == weapon.TargetUnit.GetType())
        {
            unit.SetDamage(damage);
        }

        //if (unit is PlayerCharacter && unit.Health == 0)
        //{
        //    isActive = false;
        //    transform.SetParent(other.transform);
        //}
        //else
            Reset();
    }
}
