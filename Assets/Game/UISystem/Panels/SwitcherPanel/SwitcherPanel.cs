using System.Linq;
using UnityEngine;

public class SwitcherPanel : AbstractPanel
{
    [SerializeField] private SwitcherPanelButton[] panelButtons;

    private SwitcherPanelButton currentPanelButton;
    private float duration = 0.1f;

    public override PanelType Type => PanelType.Switcher;

    public override void Init()
    {
        foreach (var button in panelButtons)
        {
            button.Init();

            // TODO: implement UnlockPanelsSO
            if (button.Type != SwitcherButtonType.Battle && button.Type != SwitcherButtonType.Upgrade)
            {
                button.Deactivate();
                button.Lock();
            }
        }

        currentPanelButton = panelButtons.First(p => p.Type == SwitcherButtonType.Battle);
        currentPanelButton.ChangeSize(duration, true);
        currentPanelButton.Deactivate();

        Subscribe();
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