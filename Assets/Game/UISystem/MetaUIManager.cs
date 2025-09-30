using UnityEngine;

public class MetaUIManager : UIManager
{
    //TODO: implement init logic
    public void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        EnterStartBattlePanel();

        Subscribe();
    }

    #region Enters
    public void EnterStartBattlePanel()
    {
        HideAllPanels();
        GetPanel(PanelType.Start).Show();
        GetPanel(PanelType.Switcher).Show();
    }

    public void EnterUpgradePanel()
    {
        HideAllPanels();
        GetPanel(PanelType.PlayerUpgrades).Show();
        GetPanel(PanelType.Switcher).Show();
    }
    #endregion

    #region Events
    private void Subscribe()
    {
        EventBus<SwitcherPanelEventInvoker>.Subscribe(SwitchInvoke);
    }

    private void Unsubscribe()
    {
        EventBus<SwitcherPanelEventInvoker>.Unsubscribe(SwitchInvoke);
    }

    private void SwitchInvoke(SwitcherPanelEventInvoker invoke)
    {
        switch (invoke.SwitcherButtonType)
        {
            case SwitcherButtonType.UnknowOne:
                break;
            case SwitcherButtonType.Upgrade:
                EnterUpgradePanel();
                break;
            case SwitcherButtonType.Battle:
                EnterStartBattlePanel();
                break;
            case SwitcherButtonType.UnknowTwo:
                break;
            case SwitcherButtonType.UnknowThree:
                break;
        }
    }
    #endregion

    private void OnDestroy()
    {
        Unsubscribe();
    }
}