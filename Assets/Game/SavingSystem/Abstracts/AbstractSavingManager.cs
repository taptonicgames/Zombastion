using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class AbstractSavingManager
{
    protected DateTime lastSaveDate;
    protected Dictionary<SavingDataType, AbstractSavingData> savingDataPairs =
        new Dictionary<SavingDataType, AbstractSavingData>();
    protected int sceneIndex;
    private bool dontSave;

    public string CustomSavingDataPath => GetCustomSavingDataPath();
    public bool IsSavingDataLoadComplete { get; private set; }
    public bool DontSave
    {
        get => dontSave;
    }

    protected AbstractSavingManager(bool dontSave)
    {
        this.dontSave = dontSave;
    }

    protected virtual void Init()
    {
        AddSavingDatasToList();
    }

    public virtual AbstractSavingData GetSavingData(SavingDataType type)
    {
        return savingDataPairs[type];
    }

    public virtual T GetSavingData<T>(SavingDataType type)
        where T : AbstractSavingData
    {
        return (T)savingDataPairs[type];
    }

    protected virtual void LoadES3Data<T>(T data, string path = "")
        where T : AbstractSavingData
    {
        if (path == "")
        {
            if (ES3.KeyExists(data.ToString()))
                ES3.LoadInto(data.ToString(), data);
        }
        else
        {
            if (ES3.KeyExists(data.ToString(), path))
                ES3.LoadInto(data.ToString(), path, data);
        }

        data.LoadData();
    }

    protected abstract void AddSavingDatasToList();

    public virtual void LoadData()
    {
        IsSavingDataLoadComplete = true;
    }

    public virtual void SaveData(bool collectParams = true)
    {
        lastSaveDate = DateTime.UtcNow;
    }

    public virtual void SaveAllData()
    {
        var b = dontSave;
        dontSave = false;
        ES3.Save("sceneIndex", sceneIndex);

        if (!savingDataPairs.Any())
            AddSavingDatasToList();

        foreach (var item in savingDataPairs.Values)
        {
            item.SaveData(true);
        }

        dontSave = b;
        Debug.Log("Saved");
    }

    public virtual void SaveDataToPath(string key)
    {
        foreach (var item in savingDataPairs.Values)
        {
            item.SaveDataToPath(true, key);
        }

        ES3.Save("sceneIndex", sceneIndex, $"SaveFile_{key}.es3");
    }

    public void DestroyData()
    {
        ES3.DeleteFile();

        for (int i = 0; i < 100; i++)
        {
            ES3.DeleteFile($"ResourceProducers_{i}.es3");
        }

#if UNITY_EDITOR
        if (EditorApplication.isPlaying)
            dontSave = true;
#endif
        Debug.Log("Saved Data Destroyed");
    }

    public void ResetSavingData()
    {
        foreach (var item in savingDataPairs.Values)
        {
            item.ResetData();
        }
        dontSave = true;
    }

    public static string GetCustomSavingDataPath()
    {
        return "CustomSavingData.es3";
    }

    public virtual bool IsSavingDatasEmpty() => false;

    protected virtual void OnApplicationFocus(bool focus)
    {
        if (SceneManager.GetActiveScene().name == "Loading Scene")
            return;
        //if (!focus) SaveData();
    }

    protected virtual void OnApplicationQuit()
    {
        if (SceneManager.GetActiveScene().name == "Loading Scene")
            return;
        if ((DateTime.UtcNow - lastSaveDate).TotalSeconds > 0.5f)
            SaveData();
    }
}
