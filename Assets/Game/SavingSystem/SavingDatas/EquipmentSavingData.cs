using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSavingData : AbstractSavingData
{
    [field: SerializeField] public Dictionary<string, int> BagEquipmentPairs { get; set; } = new Dictionary<string, int>();
    [field: SerializeField] public Dictionary<EquipmentType, string> EquipmentPairs { get; set; } = new Dictionary<EquipmentType, string>();
    [field: SerializeField] public Dictionary<string, int> BagInsertPairs { get; set; } = new Dictionary<string, int>();
    [field: SerializeField] public Dictionary<string, EquipmentType> EquipmentInsertPairs { get; set; } = new Dictionary<string, EquipmentType>();

    public override void ResetData(int flag = 0)
    {

    }

    public void AddClothData(EquipmentData equipmentData)
    {
        if (BagEquipmentPairs.ContainsKey(equipmentData.Id) == false)
            BagEquipmentPairs.Add(equipmentData.Id, 1);
        else
            BagEquipmentPairs[equipmentData.Id] += 1;

        SaveData(false);
    }

    public void RemoveClothData(EquipmentData equipmentData)
    {
        if (BagEquipmentPairs.ContainsKey(equipmentData.Id))
        {
            BagEquipmentPairs[equipmentData.Id] -= 1;

            if (BagEquipmentPairs[equipmentData.Id] <= 0)
                BagEquipmentPairs.Remove(equipmentData.Id);
        }
        SaveData(false);
    }

    public void AddEquipment(EquipmentType type, string id)
    {
        if (EquipmentPairs.ContainsKey(type) == false)
            EquipmentPairs.Add(type, id);

        SaveData(false);
    }

    public void AddInsertAtBag(InsertData insertData)
    {
        if (BagInsertPairs.ContainsKey(insertData.Id) == false)
            BagInsertPairs.Add(insertData.Id, 1);
        else
            BagInsertPairs[insertData.Id] += 1;
        SaveData(false);
    }

    public void RemoveInsertAtBag(InsertData insertData)
    {
        if (BagInsertPairs.ContainsKey(insertData.Id))
        {
            BagInsertPairs[insertData.Id] -= 1;

            if (BagInsertPairs[insertData.Id] < 1)
                BagInsertPairs.Remove(insertData.Id);
        }

        SaveData(false);
    }

    public void EquipInsert(InsertData insertData, EquipmentType equipmentType)
    {
        if (EquipmentInsertPairs.ContainsKey(insertData.Id) == false)
            EquipmentInsertPairs.Add(insertData.Id, equipmentType);

        SaveData(false);
    }

    public void UnequipInsert(InsertData insertData)
    {
        if (EquipmentInsertPairs.ContainsKey(insertData.Id))
            EquipmentInsertPairs.Remove(insertData.Id);

        SaveData(false);
    }

    protected override void SaveDataObject()
    {
        ES3.Save(ToString(), this);
    }
}