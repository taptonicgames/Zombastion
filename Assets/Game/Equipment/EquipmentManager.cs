using System.Collections.Generic;
using System.Linq;
using Zenject;

public class EquipmentManager : IInitializable
{
    [Inject] private EquipmentPackSO equipmentPackSO;

    private List<EquipmentData> equipmentDatas = new List<EquipmentData>();
    private List<EquipmentData> clothDatas = new List<EquipmentData>();
    private List<InsertData> insertDatas = new List<InsertData>();

    public void Initialize()
    {
        for (int i = 0; i < equipmentPackSO.StartEquipments.Length; i++)
        {
            EquipmentData data = new EquipmentData(equipmentPackSO.StartEquipments[i]);
            equipmentDatas.Add(data);
        }

        //TODO: Load saving equipment datas
        for (int i = 0; i < equipmentPackSO.Equipments.Length; i++)
        {
            EquipmentData data = new EquipmentData(equipmentPackSO.Equipments[i]);
            clothDatas.Add(data);
        }

        //TODO: Load saving insert datas
        for (int i = 0; i < equipmentPackSO.Inserts.Length; i++)
        {
            InsertEquipUIData insertEquipUIData = GetInsertEquipUIData(equipmentPackSO.Inserts[i].EquipmentType);
            InsertRarityUIData insertRarityUIData = GetInsertRarityUIData(equipmentPackSO.Inserts[i].Rarity);
            InsertData data = new InsertData(insertRarityUIData, equipmentPackSO.Inserts[i].EquipmentType, equipmentPackSO.Inserts[i].Rarity, insertEquipUIData);
            insertDatas.Add(data);
        }
    }

    public List<EquipmentData> GetEquipmentDatas()
    {
        return equipmentDatas;
    }

    public List<EquipmentData> GetClothDatas()
    {
        return clothDatas;
    }

    public List<InsertData> GetInsertDatas()
    {
        return insertDatas;
    }

    public List<InsertData> GetEquipInsertDatas()
    {
        List<InsertData> datas = new List<InsertData>();

        foreach (var data in equipmentDatas)
            for (int i = 0; i < data.InsertDatas.Length; i++)
                if (data.InsertDatas[i] != null)
                    datas.Add(data.InsertDatas[i]);

        return datas;
    }

    public EquipmentData GetEquipmentDataByID(string id)
    {
        return equipmentDatas.FirstOrDefault(i => i.Id == id);
    }

    public EquipmentData GetEquipmentDataByType(EquipmentType type)
    {
        return equipmentDatas.FirstOrDefault(i => i.Type == type);
    }

    public InsertRarityUIData GetInsertRarityUIData(EquipmentRarity equipmentRarity)
    {
        return equipmentPackSO.InsertRarityUIDatas.FirstOrDefault(i => i.Rarity == equipmentRarity);
    }

    public InsertEquipUIData GetInsertEquipUIData(EquipmentType type)
    {
        return equipmentPackSO.InsertEquipUIDatas.FirstOrDefault(i => i.Type == type);
    }

    public void UpdateEquipmentData(EquipmentData currentEquipmentData, EquipmentData targetEquipmentData)
    {
        for (int i = 0; i < currentEquipmentData.InsertDatas.Length; i++)
        {
            targetEquipmentData.SetInsert(currentEquipmentData.InsertDatas[i]);
        }

        equipmentDatas.Remove(currentEquipmentData);
        equipmentDatas.Add(targetEquipmentData);
    }
}