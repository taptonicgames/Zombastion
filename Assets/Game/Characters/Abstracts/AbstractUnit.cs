using System;
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
}
