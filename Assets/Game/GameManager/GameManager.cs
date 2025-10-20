using System;
using UniRx;
using Zenject;

public class GameManager : IInitializable, IDisposable, IFixedTickable
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
    private Timer timer = new Timer(TimerMode.counterFixedUpdate);

    public void Initialize()
    {
        CreateUIManager();
        playerCharacterModel.Experience.Subscribe(OnPlayerExpChanged).AddTo(disposables);
        playerCharacterModel.Health.Subscribe(OnPlayerHealthChanged).AddTo(disposables);
        EventBus<UpgradeChoosenEvnt>.Subscribe(OnUpgradeChoosenEvnt);

        timer.OnTimerReached += () =>
            EventBus<RoundCompleteEvnt>.Publish(new() { type = RoundCompleteType.Win });

        timer.StartTimer(gamePreferences.roundDuration);
    }

    private void OnPlayerHealthChanged(int value)
    {
        if (value == 0)
		{
            timer.StopTimer();
			EventBus<RoundCompleteEvnt>.Publish(new() { type = RoundCompleteType.Fail });
		}
	}

    public void FixedTick()
    {
        timer.TimerUpdate();
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
        if (value >= gamePreferences.pointsAmountToCompleteRoundLevel)
        {
            EventBus<SetGamePauseEvnt>.Publish(new() { paused = true });
            EventBus<ExperienceReachedEvnt>.Publish(new());
        }
    }

    private void OnUpgradeChoosenEvnt(UpgradeChoosenEvnt evnt)
    {
        EventBus<SetGamePauseEvnt>.Publish(new() { paused = false });
        playerCharacterModel.ResetParameters();
    }

    public void Dispose()
    {
        disposables.Dispose();
        EventBus<RoundCompleteEvnt>.Dispose();
    }
}
