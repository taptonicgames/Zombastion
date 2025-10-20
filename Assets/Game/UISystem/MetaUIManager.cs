using System;
using UnityEngine;
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
        GetPanel(PanelType.Wallets).Show();
    }

    public void EnterPlayerUpgradePanel()
    {
        HideAllPanels();
        GetPanel(PanelType.PlayerUpgrades).Show();
        GetPanel(PanelType.Switcher).Show();
        GetPanel(PanelType.Wallets).Show();
    }

    public void EnterShopPanel()
    {
        HideAllPanels();
        GetPanel(PanelType.ShopPanel).Show();
        GetPanel(PanelType.Switcher).Show();
        GetPanel(PanelType.Wallets).Show();
    }

    public void EnterCastleUpgradePanel()
    {
        HideAllPanels();
        GetPanel(PanelType.CastleUpgrades).Show();
        GetPanel(PanelType.Switcher).Show();
        GetPanel(PanelType.Wallets).Show();
    }

    private void EnterChangePlayerPanel()
    {
        HideAllPanels();
        GetPanel(PanelType.ChangePlayerCharacter).Show();
    }

    private void EnterSkillsTreePanel()
    {
        HideAllPanels();
        GetPanel(PanelType.SkillsTree).Show();
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
        var action = GetAction(evnt.type);
        action?.Invoke();
    }

    private Action GetAction(PanelType type)
    {
        return type switch
        {
            PanelType.ShopPanel => EnterShopPanel,
            PanelType.PlayerUpgrades => EnterPlayerUpgradePanel,
            PanelType.Start => EnterStartBattlePanel,
            PanelType.CastleUpgrades => EnterCastleUpgradePanel,
            PanelType.ChangePlayerCharacter => EnterChangePlayerPanel,
            PanelType.SkillsTree => EnterSkillsTreePanel,
            _ => null
        };
    }
    #endregion

    #region Test
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetPanel(PanelType.Switcher).Hide();
            LoseRoundPanel loseRoundPanel = GetPanel(PanelType.LoseRound) as LoseRoundPanel;
            loseRoundPanel.Init(null);
            GetPanel(PanelType.LoseRound).Show();
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            GetPanel(PanelType.Switcher).Hide();
            WinRoundPanel loseRoundPanel = GetPanel(PanelType.WinRound) as WinRoundPanel;
            loseRoundPanel.Init(null);
            GetPanel(PanelType.WinRound).Show();
        }
    }
    #endregion

    private void OnDestroy()
    {
        Unsubscribe();
    }
}