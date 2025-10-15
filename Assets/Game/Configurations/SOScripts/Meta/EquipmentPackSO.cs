using System;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "EquipmentPackSO", menuName = "Installers/ScriptableObjects/EquipmentPackSO")]
public class EquipmentPackSO : ScriptableObjectInstaller<EquipmentPackSO>
{
    [field: SerializeField] public EquipmentSO[] StartEquipments {  get; private set; }
    [field: SerializeField] public EquipmentSO[] Equipments {  get; private set; }
    [field: SerializeField] public InsertSO[] Inserts {  get; private set; }
    [field: SerializeField] public InsertRarityUIData[] InsertRarityUIDatas {  get; private set; }
    [field: SerializeField] public InsertEquipUIData[] InsertEquipUIDatas {  get; private set; }

    public override void InstallBindings()
    {
        Container.BindInstance(this).AsTransient();
    }
}
