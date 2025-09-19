using Cysharp.Threading.Tasks;

public class Gun : AbstractWeapon
{
    public override void Fire()
    {
        if (inFire)
            return;

        base.Fire();
        Shoot().Forget();
    }

    private async UniTask Shoot()
    {
        while (inFire)
        {
            var bullet = objectPoolSystem.GetPoolableObject<Bullet>(
                WeaponSOData.BulletType.ToString()
            );

            bullet.transform.position = transform.position;
            bullet.transform.rotation = transform.rotation;
            bullet.Init(this, objectPoolSystem);
            await UniTask.WaitForSeconds(0.05f);
        }
    }
}
