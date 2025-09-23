using Cysharp.Threading.Tasks;

public class Bow : AbstractWeapon
{
    public override void Fire(AbstractUnit targetUnit)
    {
        if (inFire)
            return;

        base.Fire(targetUnit);
        Shoot().Forget();
    }

    private async UniTask Shoot()
    {
        await UniTask.WaitUntil(
            () => angleToTarget < Constants.ALMOST_ZERO,
            cancellationToken: targetUnit.destroyCancellationToken
        );

        await UniTask.WaitWhile(() => inReload, cancellationToken: destroyCancellationToken);

        while (inFire)
        {
            var bullet = objectPoolSystem.GetPoolableObject<Bullet>(
                WeaponSOData.BulletType.ToString()
            );

            bullet.transform.position = transform.position;
            bullet.transform.rotation = transform.rotation;
            bullet.Init(this, objectPoolSystem);
            inReload = true;

            await UniTask.WaitForSeconds(
                WeaponSOData.ShootDelay,
                cancellationToken: destroyCancellationToken
            );

            inReload = false;
        }
    }
}
