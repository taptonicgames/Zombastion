using Zenject;

public class SavingManager : AbstractSavingManager, IInitializable
{
    [Inject]
    private readonly DiContainer diContainer;

    public SavingManager(bool dontSave)
        : base(dontSave) { }

    public override bool IsSavingDatasEmpty()
    {
        return GetSavingData(SavingDataType.Player).IsDataEmpty();
    }

    protected override void AddSavingDatasToList()
    {
        savingDataPairs.Add(SavingDataType.Battle, diContainer.Instantiate<BattleSavingData>());
        savingDataPairs.Add(SavingDataType.Towers, new TowersSavingData());
        savingDataPairs.Add(SavingDataType.General, new GeneralSavingData());
        savingDataPairs.Add(SavingDataType.Currency, new CurrencySavingData());
        savingDataPairs.Add(SavingDataType.Chests, new ChestsSavingData());
        savingDataPairs.Add(SavingDataType.Equipment, new EquipmentSavingData());

        foreach (var item in savingDataPairs.Values)
        {
            item.Init(this);
        }
    }

    public override void LoadData()
    {
        Init();
        LoadES3Data(GetSavingData<BattleSavingData>(SavingDataType.Battle));
        LoadES3Data(GetSavingData<TowersSavingData>(SavingDataType.Towers));
        LoadES3Data(GetSavingData<GeneralSavingData>(SavingDataType.General));
        LoadES3Data(GetSavingData<CurrencySavingData>(SavingDataType.Currency));
        LoadES3Data(GetSavingData<ChestsSavingData>(SavingDataType.Chests));
        LoadES3Data(GetSavingData<EquipmentSavingData>(SavingDataType.Equipment));
    }

    public void Initialize()
    {
        LoadData();
    }
}
