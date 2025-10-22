using System;
using UnityEngine;

public abstract class AbstractUnitAction : IDisposable
{
	protected AbstractUnit unit;
	protected UnitActionType actionType;

	public AbstractUnitAction(AbstractUnit unit)
	{
		this.unit = unit;
		SetActionType();
	}

	public UnitActionType GetActionType()
	{
		return actionType;
	}

	public abstract bool CheckAction();

	public virtual void StartAction()
	{
		if (unit.UnitAction != null && unit.UnitAction != this)
		{
			unit.UnitAction.OnFinish();
			unit.UnitAction.OnFinish(actionType);
		}

		unit.UnitActionType = actionType;
		LogUnitAction(unit.UnitActionType);
		unit.UnitAction = this;
	}

	public void LogUnitAction(UnitActionType actionType)
	{
		if (unit.LogUnitActionType) Debug.Log($"{unit.name}, {actionType}");
	}

	public Collider[] FindNearColliders(float dist)
	{
		var arr = Physics.OverlapSphere(unit.transform.position, dist);
		return arr;
	}

	protected void ObjectFinishTurning(Vector3 direction, float clamp = 360)
	{
		var point = unit.transform.position + direction;
		var targetLocalPos = unit.transform.InverseTransformPoint(point);
		var A = targetLocalPos.x;
		var B = targetLocalPos.z;
		var alpha = Mathf.Atan2(A, B) * Mathf.Rad2Deg;
		alpha = Mathf.Clamp(alpha, -clamp, clamp);
		unit.transform.Rotate(0, alpha, 0);
	}

	public virtual void Update() { }
	public virtual void FixedUpdate() { }
	protected abstract void SetActionType();
	public virtual void SetAnimationPhase(int value) { }
	public virtual void SetTarget(Vector3 targetPos) { }
	public virtual void SetTarget(Transform targetTr) { }
	public virtual void OnFinish() { }
	public virtual void OnFinish(UnitActionType actionTypeWhoCallFinish) { }
	public virtual void Dispose() { }
}
