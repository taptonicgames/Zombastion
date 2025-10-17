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
    private List<SkillTreeData> datas = new List<SkillTreeData>();
    public List<LevelSkillButton> Buttons { get; private set; } = new List<LevelSkillButton>();

    public RectTransform RectTransform => transform.GetComponent<RectTransform>();

    public event Action<LevelSkillButton> ButtonClicked; 

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

    public void AddSkill(SkillTreeData data)
    {
        datas.Add(data);
        LevelSkillButton button = Instantiate(levelSkillButtonPrefab, buttonsContainer);
        button.Init(data);
        button.ButtonClicked += OnButtonClicked;
        Buttons.Add(button);
    }

    public void ChangeReceivedState(bool isReceived)
    {
        foreach (var button in Buttons)
            button.ChangeReceivedState(isReceived);
    }

    private void OnButtonClicked(LevelSkillButton button)
    {
        ButtonClicked?.Invoke(button);
    }

    private void OnDestroy()
    {
        foreach (var button in Buttons)
            button.ButtonClicked -= OnButtonClicked;
    }
}