using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSkillBaseSO", menuName = "ScriptableObjects/LevelSkillBaseSO")]
public class LevelSkillBaseSO : ScriptableObject
{
    [field: SerializeField] public string Id { get; private set; }
    [field: SerializeField] public int level { get; private set; }
    [field: SerializeField] public float Value { get; private set; }

    [field: Space(10), Header("UI")]
    [field: SerializeField] public string Tittle { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public string Prefix { get; private set; }
    [field: SerializeField] public string Postfix { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
}