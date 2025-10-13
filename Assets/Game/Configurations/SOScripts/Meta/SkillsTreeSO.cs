using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "SkillsTreeSO", menuName = "Installers/ScriptableObjects/SkillsTreeSO")]
public class SkillsTreeSO : ScriptableObjectInstaller<SkillsTreeSO>
{
    [field: SerializeField] public int SkillsCountPerLevel { get; private set; }
    [field: SerializeField] public SkillTreeData[] Datas { get; private set; }

    private void OnValidate()
    {
        foreach (var data in Datas)
            if (data.Id != data.Tittle)
                data.Id = data.Tittle;
    }

    public override void InstallBindings()
    {
        Container.BindInstance(this).AsTransient();
    }
}
