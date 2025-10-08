using System;
using UniRx;
using Zenject;

public class GameManager : IInitializable, IDisposable
{
    [Inject]
    private readonly PlayerCharacterModel playerCharacterModel;

    [Inject]
    private readonly GamePreferences gamePreferences;
    private CompositeDisposable disposables = new();

    public void Initialize()
    {
        playerCharacterModel.Experience.Subscribe(OnPlayerExpChanged).AddTo(disposables);
    }

    private void OnPlayerExpChanged(int value)
    {
        if (value >= gamePreferences.pointsAmountToCompleteRound)
        {
            EventBus<RoundCompleteEvnt>.Publish(
                new RoundCompleteEvnt() { type = RoundCompleteType.Win }
            );
        }
    }

    public void Dispose()
    {
        disposables.Dispose();
    }
}
