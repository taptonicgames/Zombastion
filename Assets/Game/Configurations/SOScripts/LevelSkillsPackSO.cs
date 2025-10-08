using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "LevelSkillsPackSO", menuName = "Installers/ScriptableObjects/LevelSkillsPackSO")]
public class LevelSkillsPackSO : ScriptableObjectInstaller<EquipmentPackSO>
{
    [field: SerializeField] public int SkillsCountPerLevel { get; private set; }
    [field: SerializeField] public LevelSkillBaseSO[] LevelSkills { get; private set; }

    public override void InstallBindings()
    {
        Container.BindInstance(this).AsTransient();
    }
}