using Cysharp.Threading.Tasks;

public class Bow : AbstractWeapon
{
    public override void Fire(AbstractUnit shootingUnit, AbstractUnit targetUnit)
    {
        if (inFire)
            return;

        base.Fire(shootingUnit, targetUnit);
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
            bullet.Init(this, objectPoolSystem, CalculateDamage());
            inReload = true;

            await UniTask.WaitForSeconds(
                CalculateReloadTime(),
                cancellationToken: destroyCancellationToken
            );

            inReload = false;
        }
    }
}
