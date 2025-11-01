using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GeneralSavingData : AbstractSavingData
{
    public Dictionary<string, int> levelSaved = new Dictionary<string, int>()
    {
        { Constants.GLOBAL_PLAYER_LEVEL, 1 },
        { Constants.ROUNDS_COMPLETED, 0 },
        { Constants.ROUND_PICKED, 0},
        { Constants.SKILL_TREE_LEVEL, 0}
    };

    public override void ResetData(int flag = 0)
    {
    }

    public int GetParamById(string id)
    {
        return levelSaved[id];
    }

    public void SetParamById(string id, int newLevel)
    {
        levelSaved[id] = newLevel;

        SaveData(false);
    }

    public override void SaveData(bool collectParams, bool isSave = true)
    {
        if (isSave == false) return;

        base.SaveData(collectParams);
    }

    protected override void SaveDataObject()
    {
        ES3.Save(ToString(), this);
    }
}
