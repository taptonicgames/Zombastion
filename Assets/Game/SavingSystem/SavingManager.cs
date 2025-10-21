using Zenject;

public class SavingManager : AbstractSavingManager, IInitializable
{
    public SavingManager(bool dontSave)
        : base(dontSave) { }

    public override bool IsSavingDatasEmpty()
    {
        return GetSavingData(SavingDataType.Player).IsDataEmpty();
    }

    protected override void AddSavingDatasToList()
    {
        savingDataPairs.Add(SavingDataType.Battle, new BattleSavingData());

        foreach (var item in savingDataPairs.Values)
        {
            item.Init(this);
        }
    }

    public override void LoadData()
    {
        Init();
        LoadES3Data(GetSavingData<BattleSavingData>(SavingDataType.Battle));
    }

    public void Initialize()
    {
        LoadData();
    }
}
