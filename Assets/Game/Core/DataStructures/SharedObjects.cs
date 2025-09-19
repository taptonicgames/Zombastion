using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public struct SharedObjects
{
    [SerializeField]
    private List<IDGameObjectData> idGameObjectDatasList;

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

    public void SetTypeGameObjectDatasList(List<IDGameObjectData> list) =>
        idGameObjectDatasList = list;
}
