using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyPanel : AbstractPanel
{
    [SerializeField] private PlayerProfileButton profileButton;
    [SerializeField] private WalletView[] walletViews;

    public override PanelType Type => PanelType.Wallets;

    public override void Init()
    {
        base.Init();

        profileButton.Init();
        walletViews[0].Init(CurrencyType.Money);
        walletViews[1].Init(CurrencyType.Gems);
        walletViews[2].Init(CurrencyType.Energy);
    }
}
