using UnityEngine;
using Zenject;

public class MetaSceneManager : IInitializable
{
    [Inject] private readonly SharedObjects sharedObjects;
    [Inject] private DiContainer diContainer;

    public void Initialize()
    {
        var prefab = sharedObjects.GetPrefab(nameof(MetaUIManager));
        var uiManager = diContainer.InstantiatePrefabForComponent<MetaUIManager>(prefab);
       
        uiManager.Init();
    }
}