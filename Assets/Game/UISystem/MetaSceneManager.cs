using System.ComponentModel;
using UnityEngine;
using Zenject;

public class MetaSceneManager : IInitializable
{
    [Inject] private readonly SharedObjects sharedObjects;
    [Inject] private DiContainer diContainer;

    public void Initialize()
    {
        var uiManager = 
            diContainer.InstantiatePrefabForComponent<MetaUIManager>(
                sharedObjects.GetPrefab(Constants.META_UI_MANAGER));

        var rawPlayerView = diContainer.InstantiatePrefabForComponent<RawPlayerView>(
                sharedObjects.GetPrefab(Constants.RAW_PLAYER_VIEW), uiManager.transform);

        uiManager.Init();
    }
}