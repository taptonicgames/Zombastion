using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Coffee.UIExtensions;

public class Card : MonoBehaviour
{
    [SerializeField] private CardRareElmentsViewer rareElmentsViewer;
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text tittle;
    [SerializeField] private TMP_Text description;
    [SerializeField] private Image back;
    [SerializeField] private Image icon;
    [SerializeField] private UIParticle uIParticle;
    [SerializeField] private UIGradient cardGradient;

    private CardAnimator animator;

    public BattleUpgradeConfig CurrentUpgrade { get; private set; }

    public Action<Card> ButtonClicked;

    public void Init()
    {
        animator = new CardAnimator(this, uIParticle);

        button.onClick.AddListener(OnButtonClicked);
    }

    public void Show(BattleUpgradeConfig currentUpgrade, BattleUpgradeStorage battleUpgradeStorage, BattleUpgradeConfigsPack upgradeConfigsPack)
    {
        CurrentUpgrade = currentUpgrade;

        tittle.SetText(CurrentUpgrade.Tittle);
        string valueText = CurrentUpgrade.Value > 0 ? $"{CurrentUpgrade.Value}" : "";
        string description = $"{CurrentUpgrade.Description} {CurrentUpgrade.Prefix}{valueText}{CurrentUpgrade.Postfix}";
        this.description.SetText(description);

        icon.sprite = CurrentUpgrade.UpgradeIcon;

        rareElmentsViewer?.ShowStarsIndicator(CurrentUpgrade, battleUpgradeStorage, upgradeConfigsPack);
    }

    public void SetInteractable(bool value)
    {
        button.interactable = value;
    }

    public void OnCardPicked(Action callback, float delay = 0.0f)
    {
        //if (CurrentUpgrade.IsAccumulating)
        //    rareElmentsViewer.OnCardReached(() => PlayCardPickAnimation(callback, delay));
        //else
            PlayCardPickAnimation(callback, delay);
    }

    private void PlayCardPickAnimation(Action callback, float delay = 0.0f)
    {
        animator.PlayPickAnimation(() => callback?.Invoke(), delay);
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
