using UnityEngine;

[CreateAssetMenu(fileName = "UpgradesSO", menuName = "ScriptableObjects/UpgradesSO")]
public class UpgradesSO : ScriptableObject
{
    [field: SerializeField] public UpgradeData[] Datas { get; private set; }

    public void OnValidate()
    {
        foreach (var data in Datas)
            data.Validate();
    }
}