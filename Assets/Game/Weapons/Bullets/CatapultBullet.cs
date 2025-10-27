public class CatapultBullet : Bullet
{
    private ObjectParabolaJumpHelper objectParabolaJumpHelper = new();

    public override void Init(AbstractWeapon weapon, ObjectPoolSystem objectPoolSystem, int damage)
    {
        base.Init(weapon, objectPoolSystem, damage);
        ObjectParabolaJumpHelper.JumpObjectData jumpObjectData = default;
        jumpObjectData.endTr = weapon.TargetUnit.transform;
        jumpObjectData.startPos = transform.position;
        jumpObjectData.destroyOnFinish = true;
        jumpObjectData.CompleteAction = CompleteAction;
        jumpObjectData.list.Add(transform);
        objectParabolaJumpHelper.JumpObjects(jumpObjectData);
    }

    private void CompleteAction(int obj)
    {
        weapon.TargetUnit.SetDamage(damage);
    }
}
