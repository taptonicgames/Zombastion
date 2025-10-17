using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public class SkillTreeData
{
    [ReadOnlyField] public string Id;
    [field: SerializeField] public float Value { get; private set; }
    [field: SerializeField] public int Price { get; private set; }

    [field: Space(10), Header("UI")]
    [field: SerializeField] public string Tittle { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public string Prefix { get; private set; }
    [field: SerializeField] public string Postfix { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
}