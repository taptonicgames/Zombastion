using StarterAssets;
using UnityEngine;

public class PonyMovementAbility : AbstractUnitAbility
{
    private readonly ThirdPersonController thirdPersonController;
    private readonly Animator ponyAnimator;

    public PonyMovementAbility(
        AbstractUnit unit,
        ThirdPersonController thirdPersonController,
        Animator ponyAnimator
    )
        : base(unit)
    {
        this.thirdPersonController = thirdPersonController;
        this.ponyAnimator = ponyAnimator;
    }

    protected override void SetAbilityType()
    {
        abilityType = AbilityType.PonyMovement;
    }

    public override void Update()
    {
        base.Update();
        var run = thirdPersonController.Input.move != Vector2.zero;
        ponyAnimator.SetBool(Constants.RUN, run);
        unit.Animator.SetBool(Constants.RUN, run);
    }
}
