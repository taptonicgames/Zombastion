using UnityEngine;

public class BattleSavingData : AbstractSavingData
{
    [field: SerializeField]
    public RoundCompleteType RoundCompleteType { get; set; }

    public override void ResetData(int flag = 0)
    {
        RoundCompleteType = RoundCompleteType.None;
    }

    public override void Init(AbstractSavingManager savingManager)
    {
        base.Init(savingManager);
        EventBus<RoundCompleteEvnt>.Subscribe(OnRoundCompleteEvnt, 10);
    }

    private void OnRoundCompleteEvnt(RoundCompleteEvnt evnt)
    {
        RoundCompleteType = evnt.type;
        SaveData(false);
    }

    protected override void SaveDataObject()
    {
        ES3.Save(ToString(), this);
    }
}
