using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothSavingData : AbstractSavingData
{
    [field: SerializeField] public List<EquipmentData> ClothDatas {  get; set; }
    [field: SerializeField] public List<InsertData> InsertDatas { get; set; }

    public override void ResetData(int flag = 0)
    {
        
    }

    public void AddClothData(EquipmentData equipmentData)
    {
        ClothDatas.Add(equipmentData);
        SaveData(false);
    }

    public void RemoveClothData(EquipmentData equipmentData)
    {
        ClothDatas.Remove(equipmentData);
        SaveData(false);
    }

    public void AddInsert(InsertData insertData)
    {
        InsertDatas.Add(insertData);
        SaveData(false);
    }

    public void RemoveInsert(InsertData insertData)
    {
        InsertDatas.Remove(insertData);
        SaveData(false);
    }

    protected override void SaveDataObject()
    {
        ES3.Save(ToString(), this);
    }
}