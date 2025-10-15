using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Coffee.UIExtensions;

public class Card : MonoBehaviour
{
    [SerializeField] private CardRareElmentsViewer _rareElmentsViewer;
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _tittle;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private TMP_Text _valueText;
    [SerializeField] private Image _back;
    [SerializeField] private Image _icon;
    [SerializeField] private UIParticle _uIParticle;
    [SerializeField] private UIGradient _cardGradient;
    [SerializeField] private float _iconAnimationDuration = 1f; // Длительность анимации иконки

    private CardAnimator _animator;
    private Vector3 _originalIconPosition;

    public BattleUpgradeConfig CurrentUpgrade { get; private set; }

    public Action<Card> ButtonClicked;

    public void Init()
    {
        _animator = new CardAnimator(this, _uIParticle);

        _button.onClick.AddListener(OnButtonClicked);

        _originalIconPosition = _icon.rectTransform.anchoredPosition;
    }

    public void Show(BattleUpgradeConfig currentUpgrade, BattleUpgradeStorage battleUpgradeStorage, BattleUpgradeConfigsPack upgradeConfigsPack)
    {
        CurrentUpgrade = currentUpgrade;

        _tittle.SetText(CurrentUpgrade.Tittle);
        _description.SetText(CurrentUpgrade.Description);

        _valueText.SetText($"{CurrentUpgrade.Prefix}{CurrentUpgrade.Value}{CurrentUpgrade.Postfix}");
        _valueText.gameObject.SetActive(CurrentUpgrade.Value > 0);
        _icon.sprite = CurrentUpgrade.UpgradeIcon;

        _rareElmentsViewer?.ShowStarsIndicator(CurrentUpgrade, battleUpgradeStorage, upgradeConfigsPack);

        AnimateIconPosition();
    }

    private void AnimateIconPosition()
    {
        // Устанавливаем начальную позицию иконки
        Vector2 startPos = _originalIconPosition;
        startPos.x = -100f;
        _icon.rectTransform.anchoredPosition = startPos;

        // Анимируем к конечной позиции
        Vector2 endPos = _originalIconPosition;
        endPos.x = -16f;
        _icon.rectTransform.DOAnchorPosX(endPos.x, _iconAnimationDuration)
            .SetEase(Ease.OutCubic)
            .SetUpdate(true);
    }

    public void SetInteractable(bool value)
    {
        _button.interactable = value;
    }

    public void OnCardPicked(Action callback, float delay = 0.0f)
    {
        if (CurrentUpgrade.IsAccumulating)
            _rareElmentsViewer.OnCardReached(() => PlayCardPickAnimation(callback, delay));
        else
            PlayCardPickAnimation(callback, delay);
    }

    private void PlayCardPickAnimation(Action callback, float delay = 0.0f)
    {
        _animator.PlayPickAnimation(() => callback?.Invoke(), delay);
    }

    private void OnButtonClicked()
    {
        ButtonClicked?.Invoke(this);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(OnButtonClicked);
    }
}
