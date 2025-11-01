using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentData : AbstractEquipment
{
    public int Level { get; private set; }
    public int Value { get; private set; }
    public int InsertAvilableAmount { get; private set; }
    public int[] EnchanceLevels { get; private set; }
    public EquipmentUIData UIData { get; private set; }
    public CurrencyType UpgradeCurrency { get; private set; }
    public InsertData[] InsertDatas { get; private set; } = new InsertData[5];
    public float GrowFactor { get; internal set; }

    public event Action LevelChanged;

    public EquipmentData(EquipmentSO equipmentSO)
    {
        Type = equipmentSO.Type;
        Rarity = equipmentSO.Rarity;
        EnchanceLevels = equipmentSO.EnchanceLevels;
        UIData = equipmentSO.UIData;
        Value = equipmentSO.Value;
        GrowFactor = equipmentSO.GrowFactor;
        UpgradeCurrency = equipmentSO.UpgradeCurrency;
        InsertAvilableAmount = equipmentSO.InsertAvilableAmount;
        Id = equipmentSO.Id;
    }

    public void SetInsert(InsertData insertData)
    {
        for (int i = 0; i < InsertDatas.Length; i++)
        {
            if (InsertDatas[i] == null)
            {
                InsertDatas[i] = insertData;
                return;
            }
        }
    }

    public void RemoveInsert(InsertData insertData)
    {
        for (int i = 0; i < InsertDatas.Length; i++)
        {
            if (InsertDatas[i] != null && InsertDatas[i].Id == insertData.Id)
            {
                InsertDatas[i] = null;
                return;
            }
        }
    }

    public void UpgradeLevel()
    {
        Level++;
        LevelChanged?.Invoke();
    }

    public bool HasEmptyInsert()
    {
        List<InsertData> insertDatas = new List<InsertData>();

        for (int i = 0; i < InsertDatas.Length; i++)
            if (InsertDatas[i] != null)
                insertDatas.Add(InsertDatas[i]);

        return insertDatas.Count < InsertDatas.Length && insertDatas.Count < InsertAvilableAmount;
    }
}