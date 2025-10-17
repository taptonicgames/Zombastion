using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbstractPopup : MonoBehaviour
{
    [SerializeField] private RectTransform canvas;
    [SerializeField] private Image bg;
    [SerializeField] private RectTransform popup;
    [SerializeField] private Button closeButton;

    private Sequence sequence;

    public bool IsShowed { get; protected set; } = true;

    public event Action CloseButtonClicked;

    protected virtual void Awake()
    {
        closeButton.onClick.AddListener(OnCloseButtonClicked);
    }

    public abstract void Init(object[] args);

    public virtual void Show(Action callback = null)
    {
        if (IsShowed)
            return;

        ForceShow();
        sequence.Kill();
        sequence = DOTween.Sequence();
        sequence.Append(bg.DOFade(1, 0.15f).From(0).SetEase(Ease.Linear));
        sequence.Append(popup.transform.DOScale(Vector3.one, 0.15f).From(Vector3.zero).SetEase(Ease.OutBack));
        sequence.OnComplete(() => callback?.Invoke());
    }

    public virtual void Hide()
    {
        if (!IsShowed)
            return;

        sequence.Kill();
        sequence = DOTween.Sequence();
        sequence.Append(popup.transform.DOScale(Vector3.zero, 0.15f).From(Vector3.one).SetEase(Ease.InBack));
        sequence.Append(bg.DOFade(0, 0.15f).From(1).SetEase(Ease.Linear));
        sequence.OnComplete(() => ForceHide());
    }

    public void ForceShow()
    {
        canvas.gameObject.SetActive(true);
        IsShowed = true;
    }

    public void ForceHide()
    {
        canvas.gameObject.SetActive(false);
        IsShowed = false;
    }

    private void OnCloseButtonClicked()
    {
        CloseButtonClicked?.Invoke();
    }

    private void OnDestroy()
    {
        sequence.Kill();
        closeButton.onClick.RemoveListener(OnCloseButtonClicked);
    }
}