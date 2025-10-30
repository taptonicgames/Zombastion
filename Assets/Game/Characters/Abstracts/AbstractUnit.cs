using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public abstract class AbstractUnit : MonoBehaviour, IDamageReciever
{
    [SerializeField]
    private bool isEnable;

    [field: SerializeField]
    public bool IsNeedSave { get; private set; }

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
    protected int health = 100;

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

    public virtual int Health
    {
        get => health;
        set => health = Mathf.Clamp(value, 0, 100);
    }

    public virtual void Init()
    {
        EventBus<SetGamePauseEvnt>.Subscribe(OnSetGamePauseEvnt);
    }

    protected virtual void OnSetGamePauseEvnt(SetGamePauseEvnt evnt)
    {
        if (!gameObject.activeSelf) return;
        if (evnt.paused)
            StartCoroutine(SetPauseCoroutine());
        else
            SetActionTypeForced(UnitActionType.Idler);
    }

    private IEnumerator SetPauseCoroutine()
    {
        yield return new WaitForSeconds(0.2f);
        if (unitActionType != UnitActionType.Die)
            SetActionTypeForced(UnitActionType.Pause);
    }

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

        foreach (var item in unitActionsList)
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
        Health = Mathf.Clamp(Health - damage, 0, 100);

        if (Health == 0)
            OnUnitDied();
    }

    public virtual void OnUnitDied()
    {
        IsEnable = false;
    }

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
        EventBus<SetGamePauseEvnt>.Unsubscribe(OnSetGamePauseEvnt);
    }

    public virtual IEnumerator MoveToTargetCoroutine(
        Vector3 pos,
        Action OnReachedDestination = null,
        float delayTime = 0
    )
    {
        yield return new WaitForSeconds(delayTime);
        if (!agent.enabled)
            yield break;
        agent.isStopped = false;
        agent.SetDestination(pos);
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => !agent.pathPending);
        yield return new WaitUntil(() =>
            !agent.enabled || agent.remainingDistance <= agent.stoppingDistance + 0.1f
        );
        if (agent.enabled)
        {
            agent.isStopped = true;
            OnReachedDestination?.Invoke();
        }
    }

    public virtual IEnumerator MoveToTargetCoroutine(
        Transform target,
        Action OnReachedDestination = null,
        float delayTime = 0
    )
    {
        yield return new WaitForSeconds(delayTime);
        if (!agent.enabled)
            yield break;
        agent.isStopped = false;
        agent.SetDestination(target.position);
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => !agent.pathPending);
        yield return new WaitUntil(() =>
            !agent.enabled || agent.remainingDistance <= agent.stoppingDistance + 0.1f
        );
        if (agent.enabled)
        {
            agent.isStopped = true;
            OnReachedDestination?.Invoke();
        }
    }

    public virtual Coroutine MoveToTarget(
        Vector3 pos,
        Action OnReachedDestination = null,
        float delayTime = 0
    )
    {
        var coroutine = StartCoroutine(MoveToTargetCoroutine(pos, OnReachedDestination, delayTime));
        return coroutine;
    }

    public virtual Coroutine MoveToTarget(
        Transform target,
        Action OnReachedDestination = null,
        float delayTime = 0
    )
    {
        var coroutine = StartCoroutine(
            MoveToTargetCoroutine(target, OnReachedDestination, delayTime)
        );
        return coroutine;
    }

    public abstract IGetAttackSOParameters GetAttackSOParameters();
    public abstract Type GetDamageRecieverType();
}
