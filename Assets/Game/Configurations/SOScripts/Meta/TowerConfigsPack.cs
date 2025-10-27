using UnityEngine;

[CreateAssetMenu(fileName = "TowerConfigsPack", menuName = "ScriptableObjects/TowerConfigsPack")]
public class TowerConfigsPack : ScriptableObject
{
    [field: SerializeField] public TowerSO[] TowerSOs { get; private set; }
}