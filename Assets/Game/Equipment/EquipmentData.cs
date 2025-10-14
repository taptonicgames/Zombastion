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
        Id = $"{Rarity}-{Type}";
    }

    public void UpgradeLevel()
    {
        Level++;
        LevelChanged?.Invoke();
    }
}