using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelSkillButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image background;
    [SerializeField] private Image icon;
    [SerializeField] private Sprite receivedSprite;
    [SerializeField] private Sprite notReceivedSprite;

    public SkillTreeData Data { get; private set; }

    public event Action<LevelSkillButton> ButtonClicked;

    private void Awake()
    {
        button.onClick.AddListener(OnButtonClicked);
    }

    public void Init(SkillTreeData data)
    {
        Data = data;

        icon.sprite = Data.Icon;
    }

    public void ChangeReceivedState(bool isReceived)
    {
        background.sprite = isReceived ? receivedSprite : notReceivedSprite;
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