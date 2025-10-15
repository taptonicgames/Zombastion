using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerExperiencePanel : AbstractPanel
{
    [Inject]
    private readonly PlayerCharacterModel playerCharacterModel;

    [Inject]
    private readonly GamePreferences gamePreferences;

    [SerializeField]
    private Slider slider;
    private CompositeDisposable disposables = new();

    public override PanelType Type => PanelType.PlayerExperience;

    public override void Init()
    {
        playerCharacterModel.Experience.Subscribe(OnPlayerExpChanged).AddTo(disposables);
        slider.maxValue = gamePreferences.pointsAmountToCompleteRoundLevel;
    }

    private void OnPlayerExpChanged(int value)
    {
        slider.value = value;
    }

    private void OnDestroy()
    {
        disposables.Dispose();
    }
}
