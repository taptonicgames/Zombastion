using UnityEngine;

public class HandWeapon : AbstractWeapon
{
    [SerializeField]
    private Collider weaponCollider;

    public override void Fire(AbstractUnit shootingUnit, AbstractUnit targetUnit)
    {
        base.Fire(shootingUnit, targetUnit);

        if (targetUnit)
            targetUnit.SetDamage(CalculateDamage());
    }

	public override void Fire(AbstractUnit shootingUnit, Transform targetTr)
	{
		base.Fire(shootingUnit, targetTr);

		if (targetTr)
			targetTr.GetComponentInParent<IDamageReciever>().SetDamage(CalculateDamage());
	}
}
