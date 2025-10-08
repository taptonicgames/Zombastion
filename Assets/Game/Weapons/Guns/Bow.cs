using System.Threading;
using Cysharp.Threading.Tasks;

public class Bow : AbstractWeapon
{
    private CancellationTokenSource cancellationToken;

    public override void Fire(AbstractUnit shootingUnit, AbstractUnit targetUnit)
    {
        if (inFire)
            return;

        base.Fire(shootingUnit, targetUnit);

        if (targetUnit)
        {
            cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(
                shootingUnit.destroyCancellationToken,
                targetUnit.destroyCancellationToken
            );
        }
        else
            cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(
                shootingUnit.destroyCancellationToken
            );

        Shoot().Forget();
    }

    private async UniTask Shoot()
    {
        await UniTask.WaitUntil(
            () => angleToTarget < Constants.ALMOST_ZERO,
            cancellationToken: cancellationToken.Token
        );

        await UniTask.WaitWhile(() => inReload, cancellationToken: cancellationToken.Token);

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
