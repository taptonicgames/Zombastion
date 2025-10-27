using Cysharp.Threading.Tasks;
using StarterAssets;
using UnityEngine;
using Zenject;

public class PlayerAttackAction : BasePlayerAttackAction
{
	private const float WEAPON_HIDE_DELAY = 0.2f;
	[Inject]
    private readonly CoroutineManager coroutineManager;
    private ThirdPersonController thirdPersonController;
    private PlayerHealAbility healAbility;
    private float angleToEnemyVertical = 360f;
    private Coroutine coroutine;

    public PlayerAttackAction(AbstractUnit unit)
        : base(unit)
    {
        thirdPersonController = unit.GetComponent<ThirdPersonController>();
        healAbility = unit.GetUnitAbility<PlayerHealAbility>(AbilityType.Heal);
    }

    public override bool CheckAction()
    {
        if (unit.UnitActionType == actionType)
        {
            if (healAbility.PlayerFindingCastleType == PlayerFindingCastleType.Left)
                return true;
            else
            {
                unit.SetActionTypeForced(UnitActionType.Idler);
                return false;
            }
        }

        if (healAbility.PlayerFindingCastleType == PlayerFindingCastleType.Left && CheckCondition())
        {
            StartAction();
            return true;
        }

        return false;
    }

    public override void StartAction()
    {
        base.StartAction();
        thirdPersonController.EnablePersonRotation = false;
        unit.Animator.SetBool(Constants.ATTACK, true);

        if (coroutine != null)
            coroutineManager.StopCoroutine(coroutine);
    }

    private bool CheckMoving()
    {
        return thirdPersonController.Input.move != Vector2.zero;
    }

    public override void Update()
    {
        unit.Weapon.gameObject.SetActive(true);
        thirdPersonController.JumpAndGravity();
        thirdPersonController.GroundedCheck();
        thirdPersonController.Move();
        base.Update();

        if (targetUnit && !CheckMoving())
        {
            thirdPersonController.EnablePersonAnimation = false;

            if (Mathf.Abs(angleToEnemy) > 1f)
                unit.Animator.SetFloat(Constants.SPEED, 3);
            else
                unit.Animator.SetFloat(Constants.SPEED, 0);
        }
        else
        {
            thirdPersonController.EnablePersonAnimation = true;
        }

        CheckTargetUnit();
    }

    protected override async UniTask WaitRotatingToTarget()
    {
        await UniTask.WaitUntil(() => angleToEnemyVertical <= Constants.ALMOST_ZERO);
        await base.WaitRotatingToTarget();
    }

    protected override void RotateToTarget()
    {
        if (!targetUnit)
            return;

        angleToEnemy = StaticFunctions.ObjectFinishTurning(
            ((Gun)unit.Weapon).ShootPoint,
            targetUnit.transform.position,
            -Constants.UNIT_ROTATION_SPEED,
            Constants.UNIT_ROTATION_SPEED,
            false
        );

        unit.transform.Rotate(0, angleToEnemy, 0);

        angleToEnemyVertical = StaticFunctions.ObjectFinishTurning(
            ((Gun)unit.Weapon).ShootPoint,
            targetUnit.transform.position + Vector3.up * 1f,
            -Constants.UNIT_ROTATION_SPEED,
            Constants.UNIT_ROTATION_SPEED,
            axis: RectTransform.Axis.Vertical
        );
    }

    public override void OnFinish()
    {
        base.OnFinish();
        unit.Animator.SetBool(Constants.ATTACK, false);
        thirdPersonController.EnablePersonRotation = true;
        thirdPersonController.EnablePersonAnimation = true;

        coroutine = coroutineManager.InvokeActionDelay(
            () => unit.Weapon.gameObject.SetActive(false),
			WEAPON_HIDE_DELAY
		);
    }
}
