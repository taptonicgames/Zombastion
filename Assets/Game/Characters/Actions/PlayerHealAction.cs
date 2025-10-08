using System;
using Zenject;

public class PlayerHealAction : AbstractUnitAction
{
    [Inject]
    private readonly UnitActionPermissionHandler unitActionPermissionHandler;

    public PlayerHealAction(AbstractUnit unit)
        : base(unit) { }

    public override bool CheckAction()
    {
        CheckHealing();
        return false;
    }

    protected override void SetActionType()
    {
        actionType = UnitActionType.Heal;
    }

    private void CheckHealing()
    {
        throw new NotImplementedException();
    }
}
