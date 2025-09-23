using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

public class ObjectPoolSystem : IInitializable
{
    [Inject]
    private readonly SharedObjects sharedObjects;
    private Dictionary<string, ObjectPool<GameObject>> pool = new();
    private Transform folder;
    public Action<GameObject> OnGameObjectGet,
        OnGameObjectRelease;

    public void Initialize()
    {
        folder = new GameObject("ObjectPoolSystemFolder").transform;
    }

    public void Reserve(string gameObjectType, int count = 10)
    {
        CheckPool(gameObjectType);

        if (pool[gameObjectType].CountInactive >= count)
            return;

        var reserve = new GameObject[count - pool[gameObjectType].CountInactive];

        for (int i = 0; i < count; i++)
            reserve[i] = pool[gameObjectType].Get();

        for (int i = 0; i < count; i++)
            pool[gameObjectType].Release(reserve[i]);
    }

    public GameObject GetPoolableObject(string gameObjectType)
    {
        CheckPool(gameObjectType);
        return pool[gameObjectType].Get();
    }

    public T GetPoolableObject<T>(string gameObjectType)
        where T : Component
    {
        CheckPool(gameObjectType);
        return pool[gameObjectType].Get().GetComponent<T>();
    }

    public void GetPoolableObjects(string gameObjectType, int count, out GameObject[] gameObjects)
    {
        CheckPool(gameObjectType);

        gameObjects = new GameObject[count];
        for (var index = 0; index < gameObjects.Length; index++)
            gameObjects[index] = pool[gameObjectType].Get();
    }

    public void GetPoolableObjects<T>(string gameObjectType, int count, out T[] gameObjects)
        where T : Component
    {
        CheckPool(gameObjectType);

        gameObjects = new T[count];
        for (var index = 0; index < gameObjects.Length; index++)
            gameObjects[index] = pool[gameObjectType].Get().GetComponent<T>();
    }

    public void ReleasePoolableObject(string gameObjectType, GameObject obj)
    {
        if (pool.ContainsKey(gameObjectType))
            pool[gameObjectType].Release(obj);
        else
            Debug.Log($"Attempt to release object '{gameObjectType}' to pool that doesn't exist");
    }

    public void ReleasePoolableObjects(string gameObjectType, IEnumerable<GameObject> objects)
    {
        if (pool.ContainsKey(gameObjectType))
        {
            foreach (var obj in objects)
                pool[gameObjectType].Release(obj);
        }
        else
            Debug.Log($"Attempt to release object '{gameObjectType}' to pool that doesn't exist");
    }

    public void ClearPool(string gameObjectType)
    {
        if (!pool.ContainsKey(gameObjectType))
            return;

        pool[gameObjectType].Clear();
        pool.Remove(gameObjectType);
    }

    private void CheckPool(string gameObjectType)
    {
        if (!pool.ContainsKey(gameObjectType))
            CreatePool(gameObjectType);
    }

    private void CreatePool(string gameObjectType, int count = 10)
    {
        if (pool.ContainsKey(gameObjectType))
            return;

        var data = sharedObjects.GetPrefabData(gameObjectType);

        var objectPool = new ObjectPool<GameObject>(
            () => CreateGameObject(data),
            actionOnGet: GetGameObject,
            actionOnRelease: ReleaseGameObject,
            defaultCapacity: count
        );

        pool.Add(gameObjectType, objectPool);
    }

    private GameObject CreateGameObject(IDGameObjectData data)
    {
        var newGameObject = UnityEngine.Object.Instantiate(data.gameObject, folder);
        newGameObject.name = data.id;
        return newGameObject;
    }

    private void GetGameObject(GameObject gameObject)
    {
        if (OnGameObjectGet != null)
            OnGameObjectGet(gameObject);
        else
            gameObject.SetActive(true);
    }

    private void ReleaseGameObject(GameObject gameObject)
    {
        if (OnGameObjectRelease != null)
            OnGameObjectRelease(gameObject);
        else
            gameObject.gameObject.SetActive(false);
    }
}
