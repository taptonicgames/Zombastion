using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class AbstractEnemy : AbstractUnit
{
    [Inject] private readonly SceneReferences sceneReferences;

	public override void Init()
	{
		agent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();
	}
}
