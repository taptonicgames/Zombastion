using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public struct SharedObjects
{
    [SerializeField]
    private List<IDGameObjectData> idGameObjectDatasList;

    [SerializeField]
    private List<IDSpriteData> idSpriteDatasList;

    [SerializeField]
    private List<IDScriptableObjectData> idScriptableObjectDatasList;

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

    public T GetScriptableObject<T>(string id)
        where T : ScriptableObject
    {
        var data = idScriptableObjectDatasList.FirstOrDefault(a => a.id == id);

        if (data.scriptableObject == null)
            throw new NullReferenceException($"{id} is not present in SharedObjects");

        return (T)data.scriptableObject;
    }

    public void SetTypeGameObjectDatasList(List<IDGameObjectData> list) =>
        idGameObjectDatasList = list;
}
