using UnityEngine;
using Zenject;

public class TowersManager : IInitializable
{
    private TowerSO[] towerSOs;
    private TowersSavingData towersSavingData;

    [Inject] private SharedObjects sharedObjects;
    [Inject] private SavingManager savingsManager;

    public void Initialize()
    {
        var config = sharedObjects.GetScriptableObject<TowerConfigsPack>(Constants.TOWER_CONFIGS_PACK);
        towerSOs = config.TowerSOs;
        towersSavingData = savingsManager.GetSavingData<TowersSavingData>(SavingDataType.Towers);
    }

    public TowerSO[] GetTowerSOs()
    {
        return towerSOs;
    }

    public int GetTowerLevel(string id)
    {
        return towersSavingData.GetCurrentLevel(id);
    }

    public float CalculateParam(string id, float paramOne, float paramTwo)
    {
        int level = towersSavingData.GetCurrentLevel(id);

        return level > 1 ? 
            paramOne * paramTwo * level :
            paramOne;
    }

    public void UpgradeTowerLevel(string id)
    {
        int level = GetTowerLevel(id);

        EventBus<TowerUpgradeEvnt>.Publish(new()
        {
            id = id,
            level = level + 1
        });
    }
}