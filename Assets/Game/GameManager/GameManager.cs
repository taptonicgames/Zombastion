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

    [Inject]
    private readonly CoroutineManager coroutineManager;
    private CompositeDisposable disposables = new();

    public void Initialize()
    {
        CreateUIManager();
        playerCharacterModel.Experience.Subscribe(OnPlayerExpChanged).AddTo(disposables);
        playerCharacterModel.Health.Subscribe(OnPlayerHealthChanged).AddTo(disposables);
        EventBus<UpgradeChoosenEvnt>.Subscribe(OnUpgradeChoosenEvnt);
        EventBus<RoundCompleteEvnt>.Subscribe(OnRoundCompleteEvnt);
        EventBus<GatesFallenEvnt>.Subscribe(OnGatesFallenEvnt);
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

    private void OnPlayerHealthChanged(int value)
    {
        if (value == 0)
        {
            coroutineManager.InvokeActionDelay(
                () => EventBus<RoundCompleteEvnt>.Publish(new() { type = RoundCompleteType.Fail }),
                2
            );
        }
    }

    private void OnUpgradeChoosenEvnt(UpgradeChoosenEvnt evnt)
    {
        EventBus<SetGamePauseEvnt>.Publish(new() { paused = false });
        playerCharacterModel.ResetParameters();
    }

    private void OnRoundCompleteEvnt(RoundCompleteEvnt evnt)
    {
        EventBus<OpenPanelEvnt>.Publish(new() { type = GetPanelType(evnt.type) });
	}

	private void OnGatesFallenEvnt(GatesFallenEvnt evnt)
	{
        EventBus<RoundCompleteEvnt>.Publish(new() { type = RoundCompleteType.Fail });
	}

    public void Dispose()
    {
        disposables.Dispose();
        EventBus<RoundCompleteEvnt>.Dispose();
    }

    private PanelType GetPanelType(RoundCompleteType type)
    {
        return type switch
        {
            RoundCompleteType.Fail => PanelType.LoseRound,
            RoundCompleteType.Win => PanelType.WinRound,
            _ => PanelType.None,
        };
    }
}
