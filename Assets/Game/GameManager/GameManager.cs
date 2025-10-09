using System;
using UniRx;
using Zenject;

public class GameManager : IInitializable, IDisposable
{
    [Inject]
    private readonly PlayerCharacterModel playerCharacterModel;

    [Inject]
    private readonly GamePreferences gamePreferences;

    [Inject]
    private readonly SharedObjects sharedObjects;

    [Inject]
    private readonly SceneReferences sceneReferences;

    [Inject]
    private readonly DiContainer diContainer;
    private CompositeDisposable disposables = new();

    public void Initialize()
    {
        CreateUIManager();
        playerCharacterModel.Experience.Subscribe(OnPlayerExpChanged).AddTo(disposables);
    }

    private void CreateUIManager()
    {
        var prefab = sharedObjects.GetPrefab(Constants.BATTLE_UI_MANAGER);

        var battleUIManager = diContainer.InstantiatePrefabForComponent<BattleUIManager>(
            prefab,
            sceneReferences.gameComponents
        );

        diContainer.BindInstance(battleUIManager).AsSingle();
        battleUIManager.Init();
    }

    private void OnPlayerExpChanged(int value)
    {
        if (value >= gamePreferences.pointsAmountToCompleteRound)
        {
            EventBus<ExperienceReachedEvnt>.Publish(new ExperienceReachedEvnt());
        }
    }

    public void Dispose()
    {
        disposables.Dispose();
        EventBus<RoundCompleteEvnt>.Dispose();
    }
}
