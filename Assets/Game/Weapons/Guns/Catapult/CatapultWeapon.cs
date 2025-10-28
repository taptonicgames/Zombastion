using UnityEngine;

public class CatapultWeapon : AbstractWeapon
{
    [SerializeField]
    private Transform shootPoint;

    [SerializeField]
    private Animation launchAnimation;
    private ObjectParabolaJumpHelper objectParabolaJumpHelper = new();
    private Timer reloadTimer = new Timer(TimerMode.counterFixedUpdate, false);
    private CatapultBullet bullet;

    private void Start()
    {
        CreateBullet();

        reloadTimer.OnTimerReached += () =>
        {
            IsReady = true;
            CreateBullet();
        };
    }

    private void FixedUpdate()
    {
        reloadTimer.TimerUpdate();
    }

    public override void Fire(AbstractUnit shootingUnit, AbstractUnit targetUnit)
    {
        if (inFire)
            return;

        base.Fire(shootingUnit, targetUnit);
		bullet.Init(this, objectPoolSystem, CalculateDamage());
		launchAnimation.Play();
        IsReady = false;
        reloadTimer.StartTimer(CalculateReloadTime());
    }

    private void LaunchBullet()
    {
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

    private void CreateBullet()
    {
        bullet = objectPoolSystem.GetPoolableObject<CatapultBullet>(
            WeaponSOData.BulletType.ToString()
        );

        bullet.transform.position = shootPoint.position;
        bullet.transform.SetParent(shootPoint);
    }

    public override void StopFire()
    {
        inFire = false;
    }

    public override void SetAnimationPhase(int value)
    {
        base.SetAnimationPhase(value);
        bullet.transform.SetParent(objectPoolSystem.Folder);
        LaunchBullet();
    }
}
