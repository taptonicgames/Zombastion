using System;
using UnityEngine;
using Zenject;

public class Gates : MonoBehaviour, IDamageReciever
{
    [Inject] private readonly SceneReferences sceneReferences;

	public Type GetDamageRecieverType()
	{
		return typeof(Gates);
	}

	public void SetDamage(int damage)
	{
		sceneReferences.castle.SetDamage(damage);
	}
}
