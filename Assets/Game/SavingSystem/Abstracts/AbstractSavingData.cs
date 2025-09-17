public abstract class AbstractSavingData
{
	protected AbstractSavingManager savingManager;

	public virtual void Init(AbstractSavingManager savingManager)
	{
		this.savingManager = savingManager;
	}

	public virtual void SaveData(bool collectParams, bool isSave = true)
	{
		if (savingManager.DontSave) return;
		if (isSave) SaveDataObject();
	}

	public virtual void SaveDataToPath(bool collectParams, string key)
	{
		SaveData(collectParams, false);
		SaveDataObject(key);
	}

	public virtual void LoadData() { }
	public virtual bool IsDataEmpty() { return true; }
	public abstract void ResetData(int flag = 0);
	protected abstract void SaveDataObject();

	protected virtual void SaveDataObject(string key)
	{
		//Just dublicate it in override method (except ResourceProducerSavingData)
		ES3.Save(ToString(), this, $"SaveFile_{key}.es3");
	}
}
