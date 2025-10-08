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

    [Space(10), Header("Opening the panel at the level")]
    public int OpenPlayerUpgradePanelAtLevel;
    public int OpenShopPanelAtLevel;
    public int OpenCastleUpgradePanelAtLevel;
}
