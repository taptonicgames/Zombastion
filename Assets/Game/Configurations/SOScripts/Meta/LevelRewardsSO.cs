using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "LevelRewardsSO", menuName = "ScriptableObjects/LevelRewardsSO")]
public class LevelRewardsSO : ScriptableObject
{
    [field: SerializeField] public LevelRewardData[] Datas { get; private set; }

    private void OnValidate()
    {
        for (int i = 0; i < Datas.Length; i++)
        {
            if (Datas[i].Id != $"Level_{i + 1}")
                Datas[i].Id = $"Level_{i + 1}";

            Datas[i].Validate();
        }
    }
}