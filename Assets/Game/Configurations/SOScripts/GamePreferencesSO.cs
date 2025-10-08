using System;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "GamePreferencesSO", menuName = "Installers/GamePreferencesSO")]
public class GamePreferencesSO : ScriptableObjectInstaller<GamePreferencesSO>
{
    public GamePreferences gamePreferences;

    public override void InstallBindings()
    {
        Container.BindInstance(gamePreferences).AsTransient();
    }
}

[Serializable]
public struct GamePreferences
{
    [Header("Enemies")]
    public int totalEnemiesAmount;
    public float enemySpawnDelay;
    [Header("Round")]
    public int pointsAmountToCompleteRound;

    [Space(10), Header("Opening the panel at level")]
    public int OpenShopPanelAtLevel;
    public int OpenPlayerUpgradePanelAtLevel;
    public int OpenCastleUpgradePanelAtLevel;
}
