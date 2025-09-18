using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveAction : AbstractUnitAction
{
	public EnemyMoveAction(AbstractUnit unit) : base(unit)
	{
	}

	public override bool CheckAction()
	{
		throw new System.NotImplementedException();
	}

	protected override void SetActionType()
	{
		actionType = UnitActionType.Move;
	}
}
