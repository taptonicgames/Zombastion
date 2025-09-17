using Zenject;

public class SavingManager : AbstractSavingManager, IInitializable
{
	public SavingManager(bool dontSave) : base(dontSave)
	{
	}

	public override bool IsSavingDatasEmpty()
	{
		return GetSavingData(SavingDataType.Player).IsDataEmpty();
	}

	protected override void AddSavingDatasToList()
	{
		//savingDataPairs.Add(SavingDataType.Player, new PlayerSavingData());

		foreach (var item in savingDataPairs.Values)
		{
			item.Init(this);
		}
	}

	public override void LoadData()
	{
		Init();
		//LoadES3Data(GetSavingData<PlayerSavingData>(SavingDataType.Player));
	}

	public void Initialize()
	{

	}
}
