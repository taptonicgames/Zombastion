using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TreeSkillsLevelView : MonoBehaviour
{
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private Transform lockObject;
    [SerializeField] private RectTransform buttonsContainer;
    [SerializeField] private LevelSkillButton levelSkillButtonPrefab;

    private int level;
    private List<LevelSkillBaseSO> skillBaseSOs = new List<LevelSkillBaseSO>();
    private List<LevelSkillButton> buttons = new List<LevelSkillButton>();

    public RectTransform RectTransform => transform.GetComponent<RectTransform>();

    public void Init(int level)
    {
        this.level = level;
        levelText.SetText($"{level}");
    }

    public void Lock()
    {
        lockObject.gameObject.SetActive(true);
    }

    public void Unlock()
    {
        lockObject.gameObject.SetActive(false);
    }

    public void AddSkill(LevelSkillBaseSO levelSkillBaseSO)
    {
        skillBaseSOs.Add(levelSkillBaseSO);
        LevelSkillButton button = Instantiate(levelSkillButtonPrefab, buttonsContainer);
        button.Init(levelSkillBaseSO);
        button.ButtonClicked += OnButtonClicked;
        buttons.Add(button);
    }

    private void OnButtonClicked(LevelSkillButton button)
    {
        object[] args = new object[] { button };
    }

    private void OnDestroy()
    {
        foreach (var button in buttons)
            button.ButtonClicked -= OnButtonClicked;
    }
}