using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerUpgradesPanel : AbstractPanel
{
    [SerializeField] private ClothItemsContainer equipedClothItemsContainer;
    [SerializeField] private PlayerItemsBag playerItemsBag;
    [SerializeField] private Image insertIcon;

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
    private Sequence sequence;

    [Inject] private EquipmentManager equipmentManager;
    [Inject] private CurrencyManager currencyManager;
    [Inject] private UpgradesManager upgradesManager;

    public override PanelType Type => PanelType.PlayerUpgrades;

    public override void Init()
    {
        rawPlayerView = GetComponentInParent<MetaUIManager>().GetComponentInChildren<RawPlayerView>();

        equipedClothItemsContainer.Init(equipmentManager);
        playerItemsBag.Init(equipmentManager);

        rawImage.anchoredPosition = ScreenExtension.IsPortretOrientation ? portretOrientationRawPosition : albumOrientationRawPosition;

        Subscribe();

        insertIcon.gameObject.SetActive(false);
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
        playerItemsBag.InsertEmbed += OnInsertEmbed;
        playerItemsBag.ClothReplaced += OnClothReplaced;
        clothItemPopup.InsertDataRemoved += OnInsertDataRemoved;
    }

    private void Unsubscribe()
    {
        equipedClothItemsContainer.ClothItemClicked -= OnClothItemClicked;
        clothItemPopup.CloseButtonClicked -= OnCloseButtonClicked;
        skillsTreeButton.onClick.RemoveListener(OnSkillsTreeButtonClicked);
        changeCharacterButton.onClick.RemoveListener(OnChangeCharacterButtonClicked);
        playerItemsBag.InsertEmbed -= OnInsertEmbed;
        playerItemsBag.ClothReplaced -= OnClothReplaced;
        clothItemPopup.InsertDataRemoved -= OnInsertDataRemoved;
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
        object[] objects = new object[]
        {
            item ,
            currencyManager,
            upgradesManager
        };

        clothItemPopup.Init(objects);
        clothItemPopup.Show();
    }

    private void OnCloseButtonClicked()
    {
        if (clothItemPopup.IsShowed)
            clothItemPopup.Hide();
    }

    private void OnInsertEmbed(BagInsertButton button)
    {
        InsertItemView insertItemView = equipedClothItemsContainer.GetEmptyInsertSlot(button.InsertData.Type);

        insertIcon.gameObject.SetActive(true);
        insertIcon.sprite = button.InsertData.RarityUIData.Icon;
        insertIcon.transform.SetParent(insertItemView.transform, false);
        insertIcon.transform.position = button.transform.position;
        AnimateMoving(insertIcon, () =>
        {
            insertIcon.transform.SetParent(transform);
            insertItemView.SetInsertData(button.InsertData);
            equipmentManager.GetEquipmentDataByType(button.InsertData.Type).SetInsert(button.InsertData);
            insertIcon.gameObject.SetActive(false);
        });
    }

    private void OnClothReplaced(BagEquipmentButton button)
    {
        ClothItemView clothItem = equipedClothItemsContainer.GetClothItem(button.EquipmentData.Type);

        insertIcon.gameObject.SetActive(true);
        insertIcon.sprite = button.EquipmentData.UIData.Icon;
        insertIcon.transform.SetParent(clothItem.transform, false);
        insertIcon.transform.position = button.transform.position;
        AnimateMoving(insertIcon, () =>
        {
            EquipmentData equipmentData = clothItem.EquipmentData;
            insertIcon.transform.SetParent(transform);
            equipmentManager.UpdateEquipmentData(equipmentData, button.EquipmentData);
            clothItem.Init(equipmentManager.GetEquipmentDataByType(button.EquipmentData.Type));
            clothItem.UpdateDatas();
            insertIcon.gameObject.SetActive(false);
        });
    }

    private void AnimateMoving(Image insertIcon, Action callback)
    {
        sequence.Kill();
        sequence = DOTween.Sequence();
        sequence.Append(insertIcon.transform.DOScale(Vector3.one, 0.25f).From(Vector3.zero).SetEase(Ease.OutBack));
        sequence.Append(insertIcon.transform.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.InBack));
        sequence.Join(insertIcon.transform.DOScale(Vector3.one * 0.5f, 0.5f).SetEase(Ease.Linear));
        sequence.OnComplete(() => callback());
    }

    private void OnInsertDataRemoved(InsertData data)
    {
        playerItemsBag.ShowInsert(data);
        equipedClothItemsContainer.ClearInsertSlot(data);
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