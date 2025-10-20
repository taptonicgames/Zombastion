using System;

public class EquipmentData : AbstractEquipment
{
    public int Level { get; private set; }
    public int AttackValue { get; private set; }
    public int[] EnchanceLevels { get; private set; }
    public EquipmentUIData UIData { get; private set; }
    public InsertData[] InsertDatas { get; private set; } = new InsertData[5];

    public event Action LevelChanged;

    public EquipmentData(EquipmentSO equipmentSO)
    {
        Type = equipmentSO.Type;
        Rarity = equipmentSO.Rarity;
        EnchanceLevels = equipmentSO.EnchanceLevels;
        UIData = equipmentSO.UIData;
        AttackValue = equipmentSO.Damage;
        Id = $"{Rarity}-{Type}-{equipmentSO.UIData.Tittle}";
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
}