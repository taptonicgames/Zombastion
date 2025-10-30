using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ChangePlayerPanel : AbstractPanel
{
    [SerializeField] private Transform playerInfoButtonsContainer;
    [SerializeField] private PlayerInfoViewer playerInfoViewer;

    [Space(10), Header("Buttons")]
    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private PlayerInfoButton playerInfoButtonPrefab;

    [Space(10), Header("RawImage")]
    [SerializeField] private RectTransform rawImage;
    [SerializeField] private Vector2 portretOrientationRawPosition;
    [SerializeField] private Vector2 albumOrientationRawPosition;
    [SerializeField] private Vector2 portretOrientationRawSize;
    [SerializeField] private Vector2 albumOrientationRawSize;

    private int counter = 0;
    private RawPlayerView rawPlayerView;
    private List<PlayerInfoButton> infoButtons = new List<PlayerInfoButton>();

    [Inject] private PlayerCosmeticSO playerCosmeticSO;
    [Inject] private UpgradesManager upgradesManager;
    [Inject] private CurrencyManager currencyManager;

    public override PanelType Type => PanelType.ChangePlayerCharacter;

    public override void Init()
    {
        rawPlayerView = GetComponentInParent<MetaUIManager>().GetComponentInChildren<RawPlayerView>();

        prevButton.onClick.AddListener(OnPreviousButtonClicked);
        nextButton.onClick.AddListener(OnNextButtonClicked);
        closeButton.onClick.AddListener(OnCloseButtonClicked);

        rawImage.anchoredPosition = ScreenExtension.IsPortretOrientation ? portretOrientationRawPosition : albumOrientationRawPosition;
        rawImage.sizeDelta = ScreenExtension.IsPortretOrientation ? portretOrientationRawSize : albumOrientationRawSize;

        for (int i = 0; i < playerCosmeticSO.Datas.Length; i++)
        {
            PlayerInfoButton playerInfoButton = Instantiate(playerInfoButtonPrefab, playerInfoButtonsContainer);
            playerInfoButton.Init(playerCosmeticSO.Datas[i]);
            playerInfoButton.ButtonClicked += OnPlayerInfoButtonClicked;
            infoButtons.Add(playerInfoButton);
        }

        PlayerInfoButton comingSoonInfoButton = Instantiate(playerInfoButtonPrefab, playerInfoButtonsContainer);
        comingSoonInfoButton.SetComingSoonState();

        UpdateInfo();
    }

    #region event trigger
    public void ChangeRotateActiveState(bool isActive)
    {
        rawPlayerView.ChangeRotateActiveState(isActive);
    }
    #endregion

    private void OnPlayerInfoButtonClicked(PlayerInfoButton button)
    {
        for (int i = 0; i < playerCosmeticSO.Datas.Length; i++)
            if (button.Data.id == playerCosmeticSO.Datas[i].id)
                counter = i;

        UpdateInfo();
    }

    private void OnPreviousButtonClicked()
    {
        counter--;
        if (counter < 0)
            counter = playerCosmeticSO.Datas.Length - 1;

        UpdateInfo();
    }

    private void OnNextButtonClicked()
    {
        counter++;
        if (counter > playerCosmeticSO.Datas.Length - 1)
            counter = 0;

        UpdateInfo();
    }

    private void OnCloseButtonClicked()
    {
        EventBus<OpenPanelEvnt>.Publish(
                        new OpenPanelEvnt() { type = PanelType.PlayerUpgrades });
    }

    private void UpdateInfo()
    {
        foreach (PlayerInfoButton button in infoButtons)
            button.ChangePickedState(false);

        playerInfoViewer.UpdateInfo(infoButtons[counter].Data, currencyManager, upgradesManager);
        infoButtons[counter].ChangePickedState(true);

        rawPlayerView.ChangePlayerModel(counter);
    }

    private void OnDestroy()
    {
        prevButton.onClick.RemoveListener(OnPreviousButtonClicked);
        nextButton.onClick.RemoveListener(OnNextButtonClicked);
        closeButton.onClick.RemoveListener(OnCloseButtonClicked);
    }

    #region Debug
#if UNITY_EDITOR
    private bool isPortretOrientation;

    private void Update()
    {
        if (isPortretOrientation != ScreenExtension.IsPortretOrientation)
        {
            isPortretOrientation = ScreenExtension.IsPortretOrientation;
            rawImage.anchoredPosition = ScreenExtension.IsPortretOrientation ? portretOrientationRawPosition : albumOrientationRawPosition;
            rawImage.sizeDelta = ScreenExtension.IsPortretOrientation ? portretOrientationRawSize : albumOrientationRawSize;
        }
    }
#endif
    #endregion
}