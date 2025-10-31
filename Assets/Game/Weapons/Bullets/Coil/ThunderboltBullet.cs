using System.Collections;
using UnityEngine;
using Zenject;

public class ThunderboltBullet : Bullet
{
    [Inject]
    private readonly CoroutineManager coroutineManager;

    [SerializeField]
    private LineRenderer lineRenderer;

    private const float LIGHTNING_LIFETIME = 0.2f;

    public override void Init(
        AbstractWeapon weapon,
        ObjectPoolSystem objectPoolSystem,
        int damage,
        bool isActive = true
    )
    {
        base.Init(weapon, objectPoolSystem, damage, isActive);

        if (!coroutineManager)
            weapon.diContainer.Inject(this);
    }

    public override void SetActive()
    {
        base.SetActive();
        lineRenderer.enabled = true;

        var positions = new[]
        {
            transform.position,
            weapon.TargetUnit.transform.position + Vector3.up,
        };

        lineRenderer.SetPositions(positions);
        weapon.TargetUnit.SetDamage(damage);
        coroutineManager.InvokeActionDelay(Reset, LIGHTNING_LIFETIME);
    }

    protected override IEnumerator DestroyDelay()
    {
        yield return null;
    }

    protected override void Reset()
    {
        lineRenderer.enabled = false;
        base.Reset();
    }

    protected override void Update() { }
}
