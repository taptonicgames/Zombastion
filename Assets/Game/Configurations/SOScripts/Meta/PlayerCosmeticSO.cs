using System;
using UnityEditor;
using UnityEngine;
using Zenject;

// Временный скрипт для отображения разных игроков в меню
[CreateAssetMenu(fileName = "PlayerCosmeticSO", menuName = "Installers/ScriptableObjects/PlayerCosmeticSO")]
public class PlayerCosmeticSO : ScriptableObjectInstaller<PlayerCosmeticSO>
{
    [field: SerializeField] public PlayerCosmeticData[] Datas { get; private set; }

    private void OnValidate()
    {
        foreach (var data in Datas)
            if (data.id != $"{data.Tittle}-{data.SubTittle}")
                data.id = ($"{data.Tittle}-{data.SubTittle}");
    }

    public override void InstallBindings()
    {
        Container.BindInstance(this).AsTransient();
    }

    [Serializable]
    public class PlayerCosmeticData
    {
        [ReadOnlyField] public string id;
        [field: SerializeField] public float FirstBustValue { get; private set; }
        [field: SerializeField] public float SecondBustValue { get; private set; }

        [field: Space(10), Header("UI")]
        [field: SerializeField] public string Tittle { get; private set; }
        [field: SerializeField] public string SubTittle { get; private set; }
        [field: SerializeField] public string Descriprion { get; private set; }
        [field: SerializeField] public string FirstBustDescription { get; private set; }
        [field: SerializeField] public string SecondBustDescription { get; private set; }
        [field: SerializeField] public Sprite PlayerIcon { get; private set; }
        [field: SerializeField] public Sprite SkillIcon { get; private set; }
    }
}