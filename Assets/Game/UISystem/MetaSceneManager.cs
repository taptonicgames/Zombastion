using Zenject;

public class MetaSceneManager : IInitializable
{
    [Inject] private readonly SharedObjects sharedObjects;
    [Inject] private DiContainer diContainer;
    [Inject] private AbstractSavingManager savingManager;
    [Inject] private CurrencyManager currencyManager;

    public void Initialize()
    {
        var uiManager =
            diContainer.InstantiatePrefabForComponent<MetaUIManager>(
                sharedObjects.GetPrefab(Constants.META_UI_MANAGER));

        var rawPlayerView = diContainer.InstantiatePrefabForComponent<RawPlayerView>(
                sharedObjects.GetPrefab(Constants.RAW_PLAYER_VIEW), uiManager.transform);

        TryOpenNewRound();

        uiManager.Init();
    }

    private void TryOpenNewRound()
    {
        GeneralSavingData generalSavingData = savingManager.GetSavingData<GeneralSavingData>(SavingDataType.General);
        BattleSavingData battleSavingData = savingManager.GetSavingData<BattleSavingData>(SavingDataType.Battle);

        if (battleSavingData.RoundCompleteType == RoundCompleteType.Win)
        {
            int roundsCompleteAmount = generalSavingData.GetParamById(Constants.ROUNDS_COMPLETED);
            int pickedRound = generalSavingData.GetParamById(Constants.ROUND_PICKED);

            if (roundsCompleteAmount == pickedRound)
                generalSavingData.SetParamById(Constants.ROUNDS_COMPLETED, roundsCompleteAmount + 1);
        }
    }
}