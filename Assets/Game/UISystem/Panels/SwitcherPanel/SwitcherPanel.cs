using System.Linq;
using UnityEngine;
using Zenject;

public class SwitcherPanel : AbstractPanel
{
    [SerializeField] private SwitcherPanelButton[] panelButtons;

    [Inject] private GamePreferences gamePreferences;

    private SwitcherPanelButton currentPanelButton;
    private float duration = 0.1f;

    public override PanelType Type => PanelType.Switcher;

    public override void Init()
    {
        foreach (var button in panelButtons)
        {
            button.Init();

            if (button.Type != SwitcherButtonType.Battle)
            {
                button.Deactivate();
                button.Lock();
            }
        }

        currentPanelButton = panelButtons.First(p => p.Type == SwitcherButtonType.Battle);
        currentPanelButton.ChangeSize(duration, true);
        currentPanelButton.Deactivate();

        Subscribe();

        TryOpenUpgradeButton();
    }

    private void UpdatePanelState(SwitcherPanelButton panelButton)
    {
        currentPanelButton.Activate();
        currentPanelButton.ChangeSize(duration, false);

        currentPanelButton = panelButton;

        SwitcherPanelEventInvoker switcherPanelEventInvoker = new SwitcherPanelEventInvoker(currentPanelButton.Type);
        EventBus<SwitcherPanelEventInvoker>.Publish(switcherPanelEventInvoker);

        currentPanelButton.ChangeSize(duration, true);
        currentPanelButton.Deactivate();
    }

    private void TryOpenUpgradeButton()
    {
        // TODO: implement save current level info
        var lvl = 1;

        foreach (var button in panelButtons)
        {
            if (button.IsLocked)
            {
                switch (button.Type)
                {
                    case SwitcherButtonType.Shop:
                        TryUnlockButton(lvl >= gamePreferences.OpenShopPanelAtLevel, button);
                        break;
                    case SwitcherButtonType.PlayerUpgrade:
                        TryUnlockButton(lvl >= gamePreferences.OpenPlayerUpgradePanelAtLevel, button);
                        break;
                    case SwitcherButtonType.CastleUpgrade:
                        TryUnlockButton(lvl >= gamePreferences.OpenCastleUpgradePanelAtLevel, button);
                        break;
                    case SwitcherButtonType.UnknowThree:
                        break;
                }
            }
        }
    }

    public void TryUnlockButton(bool canOpen, SwitcherPanelButton button)
    {
        if (canOpen == false)
            return;

        button.Unlock();
        button.Activate();
    }

    #region Events
    private void Subscribe()
    {
        for (int i = 0; i < panelButtons.Length; i++)
            panelButtons[i].ButtonClicked += OnButtonClicked;
    }

    private void Unsubscribe()
    {
        for (int i = 0; i < panelButtons.Length; i++)
            panelButtons[i].ButtonClicked -= OnButtonClicked;
    }

    private void OnButtonClicked(SwitcherPanelButton panelButton)
    {
        UpdatePanelState(panelButton);
    }
    #endregion

    private void OnDestroy()
    {
        Unsubscribe();
    }
}

public struct SwitcherPanelEventInvoker
{
    public SwitcherButtonType SwitcherButtonType { get; private set; }

    public SwitcherPanelEventInvoker(SwitcherButtonType switcherButtonType)
    {
        SwitcherButtonType = switcherButtonType;
    }
}