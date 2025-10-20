using System.Collections;
using UnityEngine;

public class InsertData : AbstractEquipment
{
    public DamageType DamageType { get; private set; }
    public int Damage { get; private set; }
    public InsertRarityUIData RarityUIData { get; private set; }
    public InsertEquipUIData EquipUIData { get; internal set; }

    public InsertData(InsertRarityUIData insertRarityUIData, EquipmentType equipmentType, EquipmentRarity rarity, InsertEquipUIData insertEquipUIData)
    {
        Type = equipmentType;
        Rarity = rarity;
        RarityUIData = insertRarityUIData;
        EquipUIData = insertEquipUIData;

        Id = $"{Type}-{Rarity}Insert";
    }
}