using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerUpgradesPanel : AbstractPanel
{
    [SerializeField] private ClothItemsContainer equipedClothItemsContainer;
    [SerializeField] private PlayerItemsBag playerItemsBag;

    [Space(10), Header("Popups")]
    [SerializeField] private ClothItemPopup clothItemPopup;

    [Space(10), Header("Buttons")]
    [SerializeField] private Button skillsTreeButton;
    [SerializeField] private Button changeCharacterButton;

    [Space(10), Header("RawImage")]
    [SerializeField] private RectTransform rawImage;
    [SerializeField] private Vector2 portretOrientationRawPosition;
    [SerializeField] private Vector2 albumOrientationRawPosition;

    private RawPlayerView rawPlayerView;
    
    [Inject] private EquipmentManager equipmentManager;

    public override PanelType Type => PanelType.PlayerUpgrades;

    public override void Init()
    {
        rawPlayerView = GetComponentInParent<MetaUIManager>().GetComponentInChildren<RawPlayerView>();

        equipedClothItemsContainer.Init(equipmentManager);
        playerItemsBag.Init();

        rawImage.anchoredPosition = ScreenExtension.IsPortretOrientation ? portretOrientationRawPosition : albumOrientationRawPosition;

        Subscribe();
    }

    public override async UniTask OnShow()
    {
        await UniTask.Yield();

        playerItemsBag.Show();
    }

    public override async UniTask OnHide()
    {
        await UniTask.Yield();

        rawPlayerView.Restart();

        if (clothItemPopup.IsShowed)
            clothItemPopup.ForceHide();
    }

    #region event trigger
    public void ChangeRotateActiveState(bool isActive)
    {
        rawPlayerView.ChangeRotateActiveState(isActive);
    }
    #endregion

    #region Events
    private void Subscribe()
    {
        equipedClothItemsContainer.ClothItemClicked += OnClothItemClicked;
        clothItemPopup.CloseButtonClicked += OnCloseButtonClicked;
        skillsTreeButton.onClick.AddListener(OnSkillsTreeButtonClicked);
        changeCharacterButton.onClick.AddListener(OnChangeCharacterButtonClicked);
    }

    private void OnSkillsTreeButtonClicked()
    {
        EventBus<OpenPanelEvnt>.Publish(
            new OpenPanelEvnt() { type = PanelType.SkillsTree });
    }

    private void OnChangeCharacterButtonClicked()
    {
        EventBus<OpenPanelEvnt>.Publish(
            new OpenPanelEvnt() { type = PanelType.ChangePlayerCharacter });
    }

    private void OnClothItemClicked(ClothItemView item)
    {
        object[] objects = new object[] {item};
        clothItemPopup.Init(objects);
        clothItemPopup.Show();
    }

    private void OnCloseButtonClicked()
    {
        if (clothItemPopup.IsShowed)
            clothItemPopup.Hide();
    }

    private void Unsubscribe()
    {
        equipedClothItemsContainer.ClothItemClicked -= OnClothItemClicked;
        clothItemPopup.CloseButtonClicked -= OnCloseButtonClicked;
        skillsTreeButton.onClick.RemoveListener(OnSkillsTreeButtonClicked);
        changeCharacterButton.onClick.RemoveListener(OnChangeCharacterButtonClicked);
    }
    #endregion

    private void OnDestroy()
    {
        Unsubscribe();
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
        }
    }
#endif
    #endregion
}