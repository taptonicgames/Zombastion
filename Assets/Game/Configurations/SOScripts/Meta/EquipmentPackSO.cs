using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "EquipmentPackSO", menuName = "Installers/ScriptableObjects/EquipmentPackSO")]
public class EquipmentPackSO : ScriptableObjectInstaller<EquipmentPackSO>
{
    [field: SerializeField] public EquipmentSO[] StartEquipments {  get; private set; }
    [field: SerializeField] public EquipmentSO[] Equipments {  get; private set; }

    public override void InstallBindings()
    {
        Container.BindInstance(this).AsTransient();
    }
}
