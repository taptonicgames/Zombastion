using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelSkillButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image icon;

    public LevelSkillBaseSO SkillSO { get; private set; }

    public event Action<LevelSkillButton> ButtonClicked;

    private void Awake()
    {
        button.onClick.AddListener(OnButtonClicked);
    }

    public void Init(LevelSkillBaseSO levelSkillBaseSO)
    {
        SkillSO = levelSkillBaseSO;

        icon.sprite = SkillSO.Icon;
    }

    private void OnButtonClicked()
    {
        ButtonClicked?.Invoke(this);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnButtonClicked);
    }
}