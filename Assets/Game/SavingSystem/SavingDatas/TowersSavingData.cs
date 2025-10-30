using System.Collections.Generic;
using UnityEngine;

public class TowersSavingData : AbstractSavingData
{
    private const int FIRST_LEVEL = 1;
    [field: SerializeField] public Dictionary<string, int> levelsTowers { get; set; } = new Dictionary<string, int>();

    public override void ResetData(int flag = 0) { }

    public override void Init(AbstractSavingManager savingManager)
    {
        base.Init(savingManager);

        EventBus<TowerUpgradeEvnt>.Subscribe(OnTowerUpgraded, 10);
    }

    public int GetCurrentLevel(string id)
    {
        if (levelsTowers.ContainsKey(id) == false)
            levelsTowers.Add(id, FIRST_LEVEL);

        return levelsTowers[id];
    }

    public override void SaveData(bool collectParams, bool isSave = true)
    {
        if (isSave == false) return;

        base.SaveData(collectParams);
    }

    private void OnTowerUpgraded(TowerUpgradeEvnt evnt)
    {
        levelsTowers[evnt.id] = evnt.level;
        SaveData(false);
    }

    protected override void SaveDataObject()
    {
        ES3.Save(ToString(), this);
    }
}