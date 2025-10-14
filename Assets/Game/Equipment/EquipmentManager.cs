using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EquipmentManager : IInitializable
{
    [Inject] private EquipmentPackSO equipmentPackSO;

    private List<EquipmentData> equipmentDatas = new List<EquipmentData>();

    public void Initialize()
    {
        //TODO: Load saving equipment datas
        for (int i = 0; i < equipmentPackSO.StartEquipments.Length; i++)
        {
            EquipmentData data = new EquipmentData(equipmentPackSO.StartEquipments[i]);
            equipmentDatas.Add(data);
        }
    }

    public List<EquipmentData> GetDatas()
    {
        return equipmentDatas;
    }
}