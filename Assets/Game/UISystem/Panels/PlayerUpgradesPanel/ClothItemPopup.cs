using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClothItemPopup : AbstractPopup
{
    [Header("General")]
    [SerializeField] private TMP_Text tittle;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text rareText;
    [SerializeField] private Image icon;

    [Space(10), Header("Buttons")]
    [SerializeField] private SwitchButtonsController switchButtonsController;

    [Space(10), Header("Inserts")]
    [SerializeField] private RectTransform insertsContainer;
    [SerializeField] private InsertStatsItem[] insertStatsItems;

    [Space(10), Header("Stats")]
    [SerializeField] private RectTransform statsContainer;
    [SerializeField] private TMP_Text baseAttackText;
    [SerializeField] private TMP_Text enchanceAttackText;

    [Space(10), Header("Adaptive orientation")]
    [SerializeField] private RectTransform statsView;
    [SerializeField] private Vector2 portretOrientationSize;
    [SerializeField] private Vector2 albumOrientationSize;

    protected override void Awake()
    {
        base.Awake();

        statsView.sizeDelta = ScreenExtension.IsPortretOrientation ? portretOrientationSize : albumOrientationSize;
        switchButtonsController.Init();

        Subscribe();
    }

    public override void Init(object[] args)
    {
        ClothItemView clothItem = (ClothItemView)args[0];
        tittle.SetText($"{clothItem.EquipmentData.Id}");
        rareText.SetText($"{clothItem.EquipmentData.Rarity}");
        icon.sprite = clothItem.EquipmentData.UIData.Icon;

        baseAttackText.SetText($"{clothItem.EquipmentData.AttackValue}");
        enchanceAttackText.SetText($"{clothItem.EquipmentData.EnchanceLevels[clothItem.EquipmentData.Level]}");
    }

    public override void Show(Action callback = null)
    {
        base.Show(callback);

        switchButtonsController.Show();
    }

    #region Events
    private void Subscribe()
    {
        switchButtonsController.FirstButtonClicked += OnInsertsButtonClicked;
        switchButtonsController.SecondButtonClicked += OnStatsButtonClicked;
    }

    private void Unsubscribe()
    {
        switchButtonsController.FirstButtonClicked -= OnInsertsButtonClicked;
        switchButtonsController.SecondButtonClicked -= OnStatsButtonClicked;
    }

    private void OnInsertsButtonClicked()
    {
        statsContainer.gameObject.SetActive(false);
        insertsContainer.gameObject.SetActive(true);
    }

    private void OnStatsButtonClicked()
    {
        insertsContainer.gameObject.SetActive(false);
        statsContainer.gameObject.SetActive(true);
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
            statsView.sizeDelta = ScreenExtension.IsPortretOrientation ? portretOrientationSize : albumOrientationSize;
        }
    }
#endif
    #endregion
}