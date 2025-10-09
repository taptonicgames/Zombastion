using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerSkillsTreePopup : AbstractPopup
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform levelsContainer;
    [SerializeField] private TreeSkillsLevelView treeSkillsLevelViewPrefab;

    private List<TreeSkillsLevelView> levelViews = new List<TreeSkillsLevelView>();

    [Inject] private LevelSkillsPackSO levelSkillsPackSO;

    public override void Init(object[] args)
    {
        for (int i = 0; i < levelSkillsPackSO.LevelSkills.Length / levelSkillsPackSO.SkillsCountPerLevel; i++)
        {
            TreeSkillsLevelView treeSkillsLevelView = Instantiate(treeSkillsLevelViewPrefab, levelsContainer);
            treeSkillsLevelView.Init(i + 1);
            levelViews.Add(treeSkillsLevelView);
        }

        int counter = 0;
        foreach (var levelView in levelViews)
        {
            for(int i = 0; i < levelSkillsPackSO.SkillsCountPerLevel; i++)
            {
                levelView.AddSkill(levelSkillsPackSO.LevelSkills[counter]);
                counter++;
            }
        }

        ForceHide();
    }

    public override void Show(Action callback = null)
    {
        base.Show(() =>
        scrollRect.VerticalScrollToTarget(levelsContainer.GetComponent<RectTransform>(), levelViews[0].RectTransform));
    }

}