using System;
using Zenject;

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

    private void EnterChangePlayerPanel()
    {
        HideAllPanels();
        GetPanel(PanelType.ChangePlayerCharacter).Show();
    }
    #endregion

    #region Events
    private void Subscribe()
    {
        EventBus<OpenPanelEvnt>.Subscribe(SwitchInvoke);
    }

    private void Unsubscribe()
    {
        EventBus<OpenPanelEvnt>.Unsubscribe(SwitchInvoke);
    }

    private void SwitchInvoke(OpenPanelEvnt evnt)
    {
        switch (evnt.type)
        {
            case PanelType.ShopPanel:
                EnterShopPanel();
                break;
            case PanelType.PlayerUpgrades:
                EnterPlayerUpgradePanel();
                break;
            case PanelType.Start:
                EnterStartBattlePanel();
                break;
            case PanelType.CastleUpgrades:
                EnterCastleUpgradePanel();
                break;
            case PanelType.None:
                break;
            case PanelType.ChangePlayerCharacter:
                EnterChangePlayerPanel();
                break;
        }
    }
    #endregion

    private void OnDestroy()
    {
        Unsubscribe();
    }    
}