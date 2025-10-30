using UnityEngine;
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
        savingDataPairs.Add(SavingDataType.Towers, new TowersSavingData());
        savingDataPairs.Add(SavingDataType.General, new GeneralSavingData());
        savingDataPairs.Add(SavingDataType.Currency, new CurrencySavingData());

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
    }

    public void Initialize()
    {
        LoadData();
    }
}
