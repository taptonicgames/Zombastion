using UnityEngine;

public class CatapultWeapon : AbstractWeapon
{
    [SerializeField]
    private Transform shootPoint;

    [SerializeField]
    private Animator animator;
    private ObjectParabolaJumpHelper objectParabolaJumpHelper = new();
    private Timer reloadTimer = new Timer(TimerMode.counterFixedUpdate, false);
    private CatapultBullet bullet;

    private void Start()
    {
        CreateBullet();

        reloadTimer.OnTimerReached += () =>
        {
            CreateBullet();
            //inFire = false;
            IsReady = true;
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
        animator.SetTrigger(Constants.ATTACK);
        IsReady = false;
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
        bullet = null;
    }

    private void CreateBullet()
    {
        if (bullet)
            return;

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
        if (value == 0)
        {
            bullet.transform.SetParent(objectPoolSystem.Folder);
            LaunchBullet();
        }
        else if (value == 1)
        {
            reloadTimer.StartTimer(CalculateReloadTime());
        }
    }
}
