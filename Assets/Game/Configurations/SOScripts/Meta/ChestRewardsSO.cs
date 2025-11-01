using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ChestRewardsSO", menuName = "ScriptableObjects/ChestRewardsSO")]
public class ChestRewardsSO : ScriptableObject
{
    [field: SerializeField] public ChestRewardData[] Datas { get; private set; }

    private void OnValidate()
    {
        for (int i = 0; i < Datas.Length; i++)
            if (Datas[i].Id != $"Chest_{i + 1}")
                Datas[i].Id = $"Chest_{i + 1}";
    }
}