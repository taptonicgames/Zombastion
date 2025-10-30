using UnityEngine;

[CreateAssetMenu(fileName = "UpgradesSO", menuName = "ScriptableObjects/UpgradesSO")]
public class UpgradesSO : ScriptableObject
{
    [field: SerializeField] public UpgradeData[] Datas { get; private set; }
}