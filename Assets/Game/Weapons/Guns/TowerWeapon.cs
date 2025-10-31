using UnityEngine;

public class TowerWeapon : AbstractWeapon
{
    [SerializeField]
    private Animator animator;
    private ObjectParabolaJumpHelper objectParabolaJumpHelper = new();
    private Timer reloadTimer = new Timer(TimerMode.counterFixedUpdate, false);
    private Bullet bullet;
    private Vector3 enemyPos;

    private bool LaunchOnParabola =>
        WeaponSOData.WeaponType == WeaponType.Catapult
        || WeaponSOData.WeaponType == WeaponType.Cauldron;

    private void Start()
    {
        CreateBullet();

        reloadTimer.OnTimerReached += () =>
        {
            CreateBullet();
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
        enemyPos = targetUnit.transform.position;
        bullet.Init(this, objectPoolSystem, targetUnit, CalculateDamage(), LaunchOnParabola);
        animator.SetTrigger(Constants.ATTACK);
        IsReady = false;
    }

    private void LaunchBullet()
    {
        if (LaunchOnParabola)
        {
            ObjectParabolaJumpHelper.JumpObjectData jumpObjectData = default;
            jumpObjectData.endTr = targetUnit != null ? targetUnit.transform : null;
            jumpObjectData.startPos = bullet.transform.position;
            jumpObjectData.endPos = enemyPos;
            jumpObjectData.CompleteAction += bullet.CompleteAction;
            jumpObjectData.list = new() { bullet.transform };

            jumpObjectData.duration =
                Vector3.Distance(transform.position, targetUnit.transform.position)
                / WeaponSOData.BulletSpeed;

            objectParabolaJumpHelper.JumpObjects(jumpObjectData);
        }
        else
        {
            bullet.SetActive();
            //BounceHelper bounceHelper = new(this, CalculateDamage(), targetUnit, objectPoolSystem, 3);
        }

        bullet = null;
    }

    private void CreateBullet()
    {
        if (bullet)
            return;

        bullet = objectPoolSystem.GetPoolableObject<Bullet>(WeaponSOData.BulletType.ToString());
        bullet.transform.position = ShootPoint.position;
        bullet.transform.rotation = ShootPoint.rotation;
        bullet.transform.SetParent(ShootPoint);
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
