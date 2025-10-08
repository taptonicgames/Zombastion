public class MetaUIManager : AbstractUIManager
{
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

    public void EnterPlayerUpgradePanel()
    {
        HideAllPanels();
        GetPanel(PanelType.PlayerUpgrades).Show();
        GetPanel(PanelType.Switcher).Show();
    }

    public void EnterShopPanel()
    {
        HideAllPanels();
        GetPanel(PanelType.ShopPanel).Show();
        GetPanel(PanelType.Switcher).Show();
    }

    public void EnterCastleUpgradePanel()
    {
        HideAllPanels();
        GetPanel(PanelType.CastleUpgrades).Show();
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
            case SwitcherButtonType.Shop:
                EnterShopPanel();
                break;
            case SwitcherButtonType.PlayerUpgrade:
                EnterPlayerUpgradePanel();
                break;
            case SwitcherButtonType.Battle:
                EnterStartBattlePanel();
                break;
            case SwitcherButtonType.CastleUpgrade:
                EnterCastleUpgradePanel();
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