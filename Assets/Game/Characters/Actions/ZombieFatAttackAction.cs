using UnityEngine;
using Zenject;

public class ZombieFatAttackAction : AbstractUnitAction
{
    [Inject]
    private readonly UnitActionPermissionHandler unitActionPermissionHandler;

    [Inject]
    private readonly SceneReferences sceneReferences;

    private const float DISTANCE_TO_ATTACK = 2f;

    public ZombieFatAttackAction(AbstractUnit unit)
        : base(unit) { }

    public override bool CheckAction()
    {
        if (unit.UnitActionType != actionType && CheckCondition())
            StartAction();
        if (unit.UnitActionType == actionType)
            return true;
        return false;
    }

    protected override void SetActionType()
    {
        actionType = UnitActionType.Attack;
    }

    private bool CheckCondition()
    {
        if (!unitActionPermissionHandler.CheckPermission(actionType, unit.UnitActionType))
            return false;

        if (
            Vector3.Distance(unit.transform.position, sceneReferences.castle.Gates.position)
            <= DISTANCE_TO_ATTACK
        )
        {
            return true;
        }

        return false;
    }

    public override void StartAction()
    {
        base.StartAction();
        unit.Animator.SetBool(Constants.ATTACK, true);
    }
}
