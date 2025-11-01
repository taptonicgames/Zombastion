public class InsertData : AbstractEquipment
{
    public float PercentageBonus { get; private set; }
    public InsertType InsertType { get; private set; }
    public EquipmentUIData UIData { get; private set; }

    public InsertData(InsertSO insertSO)
    {
        Id = insertSO.Id;
        PercentageBonus = insertSO.PercentageBonus;
        Type = insertSO.EquipmentType;
        InsertType = insertSO.Type;
        Rarity = insertSO.Rarity;
        UIData = insertSO.UIData;
    }
}