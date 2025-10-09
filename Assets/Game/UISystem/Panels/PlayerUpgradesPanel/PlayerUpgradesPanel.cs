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
    [SerializeField] private PlayerSkillsTreePopup skillsTreePopup;
    [SerializeField] private LevelSkillPopup levelSkillPopup;

    [Space(10), Header("Buttons")]
    [SerializeField] private Button skillsTreeButton;
    [SerializeField] private Button changeCharacterButton;

    [Space(10), Header("RawImage")]
    [SerializeField] private RectTransform rawImage;
    [SerializeField] private Vector2 portretOrientationRawPosition;
    [SerializeField] private Vector2 albumOrientationRawPosition;

    private RawPlayerView rawPlayerView;
    
    [Inject] private EquipmentPackSO equipmentPackSO;

    public override PanelType Type => PanelType.PlayerUpgrades;

    public override void Init()
    {
        rawPlayerView = GetComponentInParent<MetaUIManager>().GetComponentInChildren<RawPlayerView>();

        List<EquipmentData> datas = GetDatas();

        equipedClothItemsContainer.Init(datas);
        playerItemsBag.Init();

        skillsTreePopup.Init(null);
        levelSkillPopup.Init(null);

        rawImage.anchoredPosition = ScreenExtension.IsPortretOrientation ? portretOrientationRawPosition : albumOrientationRawPosition;

        Subscribe();
    }

    private List<EquipmentData> GetDatas()
    {
        //TODO: Load saving equipment datas
        List<EquipmentData> datas = new List<EquipmentData>();
        for (int i = 0; i < equipmentPackSO.StartEquipments.Length; i++)
        {
            EquipmentData data = new EquipmentData(equipmentPackSO.StartEquipments[i]);
            datas.Add(data);
        }

        return datas;
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
        if (skillsTreePopup.IsShowed)
            skillsTreePopup.ForceHide();
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
        skillsTreePopup.CloseButtonClicked += OnCloseButtonClicked;
    }

    private void OnSkillsTreeButtonClicked()
    {
        skillsTreePopup.Show();
    }

    private void OnChangeCharacterButtonClicked()
    {
        EventBus<OpenPanelEvnt>.Publish(
            new OpenPanelEvnt() { type = PanelType.ChangePlayerCharacter });
    }

    private void OnClothItemClicked(ClothItemView item)
    {
        object[] objects = new object[1];
        objects[0] = item;
        clothItemPopup.Init(objects);
        clothItemPopup.Show();
    }

    private void OnCloseButtonClicked()
    {
        if (skillsTreePopup.IsShowed)
            skillsTreePopup.Hide();
        if (clothItemPopup.IsShowed)
            clothItemPopup.Hide();
    }

    private void Unsubscribe()
    {
        equipedClothItemsContainer.ClothItemClicked -= OnClothItemClicked;
        clothItemPopup.CloseButtonClicked -= OnCloseButtonClicked;
        skillsTreeButton.onClick.RemoveListener(OnSkillsTreeButtonClicked);
        changeCharacterButton.onClick.RemoveListener(OnChangeCharacterButtonClicked);
        skillsTreePopup.CloseButtonClicked -= OnCloseButtonClicked;
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