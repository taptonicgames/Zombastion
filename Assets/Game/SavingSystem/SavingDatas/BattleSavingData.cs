using UnityEngine;
using Zenject;

public class BattleSavingData : AbstractSavingData
{
    private Castle castle;

    [field: SerializeField]
    public RoundCompleteType RoundCompleteType { get; set; }

    [field: SerializeField]
    public float CastleHealthRemainPercentage { get; set; }

    public override void ResetData(int flag = 0)
    {
        RoundCompleteType = RoundCompleteType.None;
    }

    public BattleSavingData(DiContainer diContainer)
    {
        var sceneReferences = diContainer.TryResolve<SceneReferences>();

        if (sceneReferences == null)
            return;

        castle = sceneReferences.castle;
    }

    public override void Init(AbstractSavingManager savingManager)
    {
        base.Init(savingManager);
        EventBus<RoundCompleteEvnt>.Subscribe(OnRoundCompleteEvnt, 10);
    }

    private void OnRoundCompleteEvnt(RoundCompleteEvnt evnt)
    {
        RoundCompleteType = evnt.type;
        //CastleHealthRemainPercentage = (float)castle.Health / (float)castle.GetDefaultHealth();
        SaveData(false);
    }

    protected override void SaveDataObject()
    {
        ES3.Save(ToString(), this);
    }
}
