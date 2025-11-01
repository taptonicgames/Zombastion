using UnityEngine;
using Zenject;

public class CurrencyPanel : AbstractPanel
{
    [SerializeField] private PlayerProfileButton profileButton;
    [SerializeField] private WalletView[] walletViews;
    [SerializeField] private PlayerInfoPopup playerInfoPopup;

    [Inject] private AbstractSavingManager savingsManager;
    [Inject] private CurrencyManager currencyManager;
    [Inject] private UpgradesManager upgradesManager;
    [Inject] private SpritesManager spritesManager;

    private GeneralSavingData generalSavingData;

    public override PanelType Type => PanelType.Wallets;

    public override void Init()
    {
        base.Init();

        generalSavingData = savingsManager.GetSavingData<GeneralSavingData>(SavingDataType.General);

        profileButton.Init(generalSavingData, currencyManager);
        walletViews[0].Init(CurrencyType.Money);
        walletViews[1].Init(CurrencyType.Gems);
        walletViews[2].Init(CurrencyType.Energy);

        profileButton.ProfileButtonClicked += OnProfileButtonClicked;
        playerInfoPopup.CloseButtonClicked += OnPupopClosed;
        playerInfoPopup.Upgraded += OnPlayerUpgraded;

        playerInfoPopup.ForceHide();
    }

    private void OnProfileButtonClicked()
    {
        object[] args = new object[]
        {
            generalSavingData,
            currencyManager,
            upgradesManager,
            spritesManager
        };

        playerInfoPopup.Init(args);
        playerInfoPopup.Show();
    }

    private void OnPupopClosed()
    {
        playerInfoPopup.Hide();
    }

    private void OnPlayerUpgraded(UpgradeData upgradeData)
    {
        int currentPlayerLevel = generalSavingData.GetParamById(Constants.GLOBAL_PLAYER_LEVEL);
        generalSavingData.SetParamById(Constants.GLOBAL_PLAYER_LEVEL, currentPlayerLevel + 1);

        for (int i = 0; i < upgradeData.Datas.Length; i++)
            currencyManager.RemoveCurrency(
                upgradeData.Datas[i].CurrencyType,
                upgradesManager.CalculatePrice(upgradeData.Datas[i], currentPlayerLevel));

        profileButton.UpdateInfo();
    }

    private void OnDestroy()
    {
        profileButton.ProfileButtonClicked -= OnProfileButtonClicked;
        playerInfoPopup.CloseButtonClicked -= OnPupopClosed;
        playerInfoPopup.Upgraded -= OnPlayerUpgraded;
    }
}
