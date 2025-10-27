using UnityEngine;

public class CatapultWeapon : AbstractWeapon
{
    [SerializeField]
    private Transform shootPoint;
    private ObjectParabolaJumpHelper objectParabolaJumpHelper = new();

    public override void Fire(AbstractUnit shootingUnit, AbstractUnit targetUnit)
    {
        base.Fire(shootingUnit, targetUnit);

        var bullet = objectPoolSystem.GetPoolableObject<CatapultBullet>(
            WeaponSOData.BulletType.ToString()
        );

        bullet.transform.position = shootPoint.position;
        bullet.Init(this, objectPoolSystem, CalculateDamage());

        ObjectParabolaJumpHelper.JumpObjectData jumpObjectData = default;
        jumpObjectData.endTr = TargetUnit.transform;
        jumpObjectData.startPos = bullet.transform.position;
        jumpObjectData.CompleteAction += bullet.CompleteAction;
        jumpObjectData.list = new() { bullet.transform };

        jumpObjectData.duration =
            Vector3.Distance(transform.position, targetUnit.transform.position)
            / WeaponSOData.BulletSpeed;

        objectParabolaJumpHelper.JumpObjects(jumpObjectData);
    }

    public override void StopFire()
    {
        inFire = false;
    }
}
