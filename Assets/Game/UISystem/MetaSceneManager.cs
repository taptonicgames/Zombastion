using UnityEngine;
using Zenject;

public class MetaSceneManager : IInitializable
{
    [Inject] private readonly SharedObjects sharedObjects;
    [Inject] private DiContainer diContainer;
    [Inject] private SavingManager savingManager;

    public void Initialize()
    {
        var uiManager = 
            diContainer.InstantiatePrefabForComponent<MetaUIManager>(
                sharedObjects.GetPrefab(Constants.META_UI_MANAGER));

        var rawPlayerView = diContainer.InstantiatePrefabForComponent<RawPlayerView>(
                sharedObjects.GetPrefab(Constants.RAW_PLAYER_VIEW), uiManager.transform);

        GeneralSavingData generalSavingData = savingManager.GetSavingData<GeneralSavingData>(SavingDataType.General);
        BattleSavingData battleSavingData = savingManager.GetSavingData<BattleSavingData>(SavingDataType.Battle);

        Debug.LogWarning(battleSavingData.RoundCompleteType + " // " + generalSavingData.GetParamById(Constants.ROUNDS_COMPLETED));
        if (battleSavingData.RoundCompleteType == RoundCompleteType.Win)
        {
            int level = generalSavingData.GetParamById(Constants.ROUNDS_COMPLETED) + 1;
            generalSavingData.SetParamById(Constants.ROUNDS_COMPLETED, level);
        }

        uiManager.Init();
    }
}