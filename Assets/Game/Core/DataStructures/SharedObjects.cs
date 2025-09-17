using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SharedObjects
{
	private List<IDGameObjectData> idGameObjectDatasList;

	public GameObject GetLootPrefab(CharacterType type)
	{
		var data = idGameObjectDatasList.FirstOrDefault(a => a.id == type.ToString());

		if (data.gameObject == null)
			throw new NullReferenceException($"{type} is not present in SharedObjects");

		return data.gameObject;
	}

	public void SetTypeGameObjectDatasList(List<IDGameObjectData> list) => idGameObjectDatasList = list;
}
