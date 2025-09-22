using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public abstract class AbstractUnit : MonoBehaviour
{
    [SerializeField]
    protected bool isEnable;

    [SerializeField]
    protected bool isNeedSave;

    [SerializeField]
    private bool logUnitActionType;

    [SerializeField]
    private CharacterType characterType;

    [Inject]
    protected readonly UnitActionPermissionHandler unitActionPermissionHandler;
    protected UnitActionType unitActionType = UnitActionType.Idler;
    private AbstractUnitAction unitAction;
    protected Animator animator;
    protected NavMeshAgent agent;
    protected List<AbstractUnitAction> unitActionsList = new List<AbstractUnitAction>();
    protected Dictionary<AbilityType, AbstractUnitAbility> abilitiesPair =
        new Dictionary<AbilityType, AbstractUnitAbility>();
    private Guid id;
    protected AbstractWeapon weapon;
    protected int health;

    public NavMeshAgent Agent
    {
        get => agent;
    }

    public Animator Animator
    {
        get => animator;
    }

    public UnitActionType UnitActionType
    {
        get => unitActionType;
        set => unitActionType = value;
    }

    public bool IsEnable
    {
        get => isEnable;
        set => isEnable = value;
    }

    public AbstractUnitAction UnitAction
    {
        get => unitAction;
        set => unitAction = value;
    }
    public bool LogUnitActionType
    {
        get => logUnitActionType;
    }
    public AbstractWeapon Weapon
    {
        get => weapon;
    }
    public CharacterType CharacterType
    {
        get => characterType;
    }

    public virtual void Init() { }

    protected virtual void Update()
    {
        foreach (var item in abilitiesPair)
        {
            item.Value.Update();
        }
        if (!IsEnable)
            return;

        if (unitActionPermissionHandler.CanIAskPermission(this))
        {
            for (int i = 0; i < unitActionsList.Count; i++)
            {
                if (unitActionsList[i].CheckAction())
                    break;
            }
        }

        if (UnitAction != null)
            UnitAction.Update();
    }

    protected virtual void FixedUpdate()
    {
        foreach (var item in abilitiesPair)
        {
            item.Value.FixedUpdate();
        }

        if (!IsEnable)
            return;

        if (UnitAction != null)
            UnitAction.FixedUpdate();
    }

    public void SetActionTypeForced(UnitActionType type)
    {
        var needAction = unitActionsList.Find(a => a.GetActionType() == type);

        if (needAction != null)
            needAction.StartAction();
        else
        {
            if (unitAction != null)
            {
                unitAction.OnFinish();
                unitAction.OnFinish(type);
                unitAction = null;
            }

            unitActionType = type;
            unitActionsList[0].LogUnitAction(type);
        }
    }

    public AbstractUnitAction GetUnitAction(UnitActionType type)
    {
        var unitAction = unitActionsList.Find(a => a.GetActionType() == type);
        return unitAction;
    }

    public virtual void SetAnimationPhase(int value)
    {
        foreach (var item in abilitiesPair.Values)
        {
            item.SetAnimationPhase(value);
        }
    }

    public virtual ReactiveProperty<T> GetReactiveProperty<T>()
        where T : struct
    {
        return default;
    }

    public Guid GetID()
    {
        if (id == Guid.Empty)
            id = Guid.NewGuid();

        return id;
    }

    public virtual void SetDamage(int damage)
    {
        health = Mathf.Clamp(health - damage, 0, 100);

        if (health == 0)
            OnUnitDied();
    }

    public virtual void OnUnitDied() { }

    public virtual T GetUnitAbility<T>(AbilityType type)
        where T : AbstractUnitAbility
    {
        return (T)abilitiesPair[type];
    }

    public void ClearUnitActions()
    {
        foreach (var item in unitActionsList)
        {
            item.Dispose();
        }

        unitActionsList.Clear();
    }

    private void OnDestroy()
    {
        ClearUnitActions();
    }

	public float ObjectFinishTurning(Vector3 targetPos, float clampMin = -10, float clampMax = 10, bool rotate = true)
	{
		var targetLocalPos = transform.InverseTransformPoint(targetPos);
		var A = targetLocalPos.x;
		var B = targetLocalPos.z;
		var alpha = Mathf.Atan2(A, B) * Mathf.Rad2Deg;
		var angle = alpha;
		alpha = Mathf.Clamp(alpha, clampMin, clampMax);
		if (rotate) transform.Rotate(0, alpha, 0);
		return angle;
	}

	public void ObjectFinishTurning(Vector3 targetPos, float duration)
	{
		var targetLocalPos = transform.InverseTransformPoint(targetPos);
		var A = targetLocalPos.x;
		var B = targetLocalPos.z;
		var alpha = Mathf.Atan2(A, B) * Mathf.Rad2Deg;
		var angle = new Vector3(0, alpha, 0);
		transform.DORotate(transform.eulerAngles + angle, duration);
	}

	public virtual IEnumerator MoveToTargetCoroutine(Vector3 pos, Action OnReachedDestination = null, float delayTime = 0)
	{
		yield return new WaitForSeconds(delayTime);
		if (!agent.enabled) yield break;
		agent.isStopped = false;
		agent.SetDestination(pos);
		yield return new WaitForSeconds(0.1f);
		yield return new WaitUntil(() => !agent.pathPending);
		yield return new WaitUntil(() => !agent.enabled || agent.remainingDistance <= agent.stoppingDistance + 0.1f);
		if (agent.enabled)
		{
			agent.isStopped = true;
			OnReachedDestination?.Invoke();
		}
	}

	public virtual IEnumerator MoveToTargetCoroutine(Transform target, Action OnReachedDestination = null, float delayTime = 0)
	{
		yield return new WaitForSeconds(delayTime);
		if (!agent.enabled) yield break;
		agent.isStopped = false;
		agent.SetDestination(target.position);
		yield return new WaitForSeconds(0.1f);
		yield return new WaitUntil(() => !agent.pathPending);
		yield return new WaitUntil(() => !agent.enabled || agent.remainingDistance <= agent.stoppingDistance + 0.1f);
		if (agent.enabled)
		{
			agent.isStopped = true;
			OnReachedDestination?.Invoke();
		}
	}

	public virtual Coroutine MoveToTarget(Vector3 pos, Action OnReachedDestination = null, float delayTime = 0)
	{
		var coroutine = StartCoroutine(MoveToTargetCoroutine(pos, OnReachedDestination, delayTime));
		return coroutine;
	}

	public virtual Coroutine MoveToTarget(Transform target, Action OnReachedDestination = null, float delayTime = 0)
	{
		var coroutine = StartCoroutine(MoveToTargetCoroutine(target, OnReachedDestination, delayTime));
		return coroutine;
	}
}
