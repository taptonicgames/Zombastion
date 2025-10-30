using System.Collections.Generic;
using UnityEngine;

public class ChestsSavingData : AbstractSavingData
{
    [field: SerializeField] public Dictionary<int, int> roundChestPairs { get; set; } = new Dictionary<int, int>();

    public override void Init(AbstractSavingManager savingManager)
    {
        base.Init(savingManager);

        EventBus<ChestOpenedEvnt>.Subscribe(OnChestOpenedEvnt, 10);
    }

    public override void ResetData(int flag = 0)
    {
    }

    public int GetChestLevel(int roundIndex)
    {
        if (roundChestPairs.ContainsKey(roundIndex) == false)
            roundChestPairs.Add(roundIndex, 0);

        return roundChestPairs[roundIndex];
    }

    private void OnChestOpenedEvnt(ChestOpenedEvnt evnt)
    {
        roundChestPairs[evnt.roundIndex] = evnt.chestLevel;
        SaveData(false);
    }

    protected override void SaveDataObject()
    {
        ES3.Save(ToString(), this);
    }
}