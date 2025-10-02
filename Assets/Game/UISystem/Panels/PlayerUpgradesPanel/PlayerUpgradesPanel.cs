using Cysharp.Threading.Tasks;
using log4net.Core;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUpgradesPanel : AbstractPanel
{
    [SerializeField] private ClothItemsContainer equipedClothItemsContainer;
    [SerializeField] private SwitchButtonsController switchButtonsController;
    [SerializeField] private RawPlayerView rawPlayerView;

    [Space(10), Header("Items containers")]
    [SerializeField] private ClothItem itemPrefab;

    [SerializeField] private Transform clothItemsBagScrollView;
    [SerializeField] private Transform clothItemsBagContainer;

    [SerializeField] private Transform insertsScrollView;
    [SerializeField] private Transform insertsContainer;

    [Space(10), Header("RawImage")]
    [SerializeField] private RectTransform rawImage;
    [SerializeField] private Vector2 portretOrientationRawPosition;
    [SerializeField] private Vector2 albomOrientationRawPosition;

    public override PanelType Type => PanelType.PlayerUpgrades;

    public override void Init()
    {
        equipedClothItemsContainer.Init();
        switchButtonsController.Init();

        rawImage.anchoredPosition = ScreenExtension.IsPortretOrientation ? portretOrientationRawPosition : albomOrientationRawPosition;

        Subscribe();
    }

    public override async UniTask OnShow()
    {
        await UniTask.Yield();

        switchButtonsController.Show();
    }

    public override async UniTask OnHide()
    {
        await UniTask.Yield();

        rawPlayerView.Restart();
    }

    #region Events
    private void Subscribe()
    {
        switchButtonsController.FirstButtonClicked += OnClothItemsBagButtonClicked;
        switchButtonsController.SecondButtonClicked += OnInsertItemsBagButtonClicked;
    }

    private void OnClothItemsBagButtonClicked()
    {
        insertsScrollView.gameObject.SetActive(false);
        clothItemsBagScrollView.gameObject.SetActive(true);
    }

    private void OnInsertItemsBagButtonClicked()
    {
        clothItemsBagScrollView.gameObject.SetActive(false);
        insertsScrollView.gameObject.SetActive(true);
    }

    private void Unsubscribe()
    {
        switchButtonsController.FirstButtonClicked -= OnClothItemsBagButtonClicked;
        switchButtonsController.SecondButtonClicked -= OnInsertItemsBagButtonClicked;
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
            rawImage.anchoredPosition = ScreenExtension.IsPortretOrientation ? portretOrientationRawPosition : albomOrientationRawPosition;
        }
    }
#endif
    #endregion
}