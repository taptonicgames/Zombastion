using UnityEngine;

[CreateAssetMenu(fileName = "ClothItemSO", menuName = "ScriptableObjects/ClothItemSO")]
public class ClothItemSO : ScriptableObject
{
    [field: Header("UI data")]
    [field: SerializeField] public string Tittle { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
}