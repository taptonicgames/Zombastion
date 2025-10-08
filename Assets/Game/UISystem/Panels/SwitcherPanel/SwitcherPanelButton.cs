using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwitcherPanelButton : MonoBehaviour
{
    [field: SerializeField] public SwitcherButtonType Type { get; private set; }
    [SerializeField] private Button button;
    [SerializeField] private Image background;
    [SerializeField] private Image icon;
    [SerializeField] private Transform[] upgradeIndicators;
    [SerializeField] private Sprite enableBackSprite;
    [SerializeField] private Sprite disableBackSprite;
    [SerializeField] private Sprite lockSprite;
    [SerializeField] private Sprite unlockSprite;
    [SerializeField] private TMP_Text text;

    [Space(10)]
    [Header("visual setting")]
    [SerializeField] private Vector2 targetPos = new Vector2(0, -65f);
    [SerializeField] private Vector2 defaultPos = new Vector2(0, -87.5f);
    [SerializeField] private Vector2 targetScale = new Vector2(90, 90);
    [SerializeField] private Vector2 defaultScale = new Vector2(112, 112);

    public RectTransform RectTransform { get; private set; }

    private Sequence sequence;

    public bool IsLocked { get; private set; }

    public event Action<SwitcherPanelButton> ButtonClicked;

    public void Init()
    {
        button.onClick.AddListener(OnButtonCliked);
        RectTransform = GetComponent<RectTransform>();
    }

    public void Activate()
    {
        if (IsLocked) return;
        button.interactable = true;
    }

    public void Deactivate()
    {
        if (IsLocked) return;
        button.interactable = false;
        ChangeActiveStateUpgradeIcon(false);
    }

    public void Lock()
    {
        IsLocked = true;
        icon.sprite = lockSprite;
    }

    public void Unlock()
    {
        IsLocked = false;
        icon.sprite = unlockSprite;
    }

    public void ChangeActiveStateUpgradeIcon(bool isActive)
    {
        if (IsLocked) return;

        for (int i = 0; i < upgradeIndicators.Length; i++)
            upgradeIndicators[i].gameObject.SetActive(isActive);
    }

    private void OnButtonCliked()
    {
        ButtonClicked.Invoke(this);
    }

    public void ChangeSize(float duration, bool isActive)
    {
        background.sprite = isActive ? enableBackSprite : disableBackSprite;

        if (!isActive)
        {
            text.gameObject.SetActive(false);
        }

        sequence.Kill();
        sequence = DOTween.Sequence();
        sequence.Join(icon.rectTransform.DOAnchorPos(isActive ? targetPos : defaultPos, duration).SetEase(Ease.Linear));
        sequence.Join(icon.rectTransform.DOSizeDelta(isActive ? targetScale : defaultScale, duration).SetEase(Ease.Linear));
        sequence.OnComplete(() => text.gameObject.SetActive(isActive));
    }

    private void OnDestroy()
    {
        sequence.Kill();
        button.onClick.RemoveListener(OnButtonCliked);
    }
}