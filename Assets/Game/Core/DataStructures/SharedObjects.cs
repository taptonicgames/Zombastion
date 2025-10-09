using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public struct SharedObjects
{
    [SerializeField]
    private List<IDGameObjectData> idGameObjectDatasList;
    [SerializeField] private List<IDSpriteData> idSpriteDatasList;
    [field: SerializeField] public BattleUpgradeConfigsPack BattleUpgradeConfigsPack { get; set; }

	public GameObject GetPrefab(string id)
    {
        var data = idGameObjectDatasList.FirstOrDefault(a => a.id == id);

        if (data.gameObject == null)
            throw new NullReferenceException($"{id} is not present in SharedObjects");

        return data.gameObject;
    }

    public IDGameObjectData GetPrefabData(string id)
    {
        var data = idGameObjectDatasList.FirstOrDefault(a => a.id == id);

        if (data.gameObject == null)
            throw new NullReferenceException($"{id} is not present in SharedObjects");

        return data;
    }

	public Sprite GetSprite(string id)
	{
		var data = idSpriteDatasList.FirstOrDefault(a => a.id == id);

		if (data.sprite == null)
			throw new NullReferenceException($"{id} is not present in SharedObjects");

		return data.sprite;
	}

	public void SetTypeGameObjectDatasList(List<IDGameObjectData> list) =>
        idGameObjectDatasList = list;
}
