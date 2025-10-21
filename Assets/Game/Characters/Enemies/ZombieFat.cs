using System;
using Zenject;

public class ZombieFat : AbstractEnemy
{
    [Inject]
    private readonly SceneReferences sceneReferences;

    private void Start()
    {
        abilitiesPair = new() { { AbilityType.Movement, new EnemyMovementAbility(this) } };

        unitActionsList = new()
        {
            new ZombieFatAttackAction(this),
            new ZombieMoveAction(this),
            new UnitIdleAction(this),
            new UnitPauseAction(this),
        };

        foreach (var item in unitActionsList)
        {
            diContainer.Inject(item);
        }

        SetActionTypeForced(UnitActionType.Move);
    }

    public override void SetAnimationPhase(int value)
    {
        base.SetAnimationPhase(value);
        sceneReferences.castle.SetDamage(SOData.Damage);
        OnUnitDied();
    }

    public override Type GetDamageRecieverType()
    {
        return typeof(ZombieFat);
    }
}
