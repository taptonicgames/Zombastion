using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClothItemPopup : AbstractPopup
{
    [SerializeField] private EnhancePopup enhancePopup;

    [Header("General")]
    [SerializeField] private TMP_Text tittle;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text rareText;
    [SerializeField] private Image icon;

    [Space(10), Header("Buttons")]
    [SerializeField] private SwitchButtonsController switchButtonsController;
    [SerializeField] private Button enhanceButton;

    [Space(10), Header("Inserts")]
    [SerializeField] private RectTransform insertsContainer;
    [SerializeField] private InsertStatsItem[] insertStatsItems;

    [Space(10), Header("Stats")]
    [SerializeField] private RectTransform statsContainer;
    [SerializeField] private TMP_Text baseAttackText;
    [SerializeField] private TMP_Text enchanceAttackText;

    private ClothItemView itemView;

    protected override void Awake()
    {
        base.Awake();

        switchButtonsController.Init();

        Subscribe();
    }

    public override void Init(object[] args)
    {
        itemView = (ClothItemView)args[0];
        tittle.SetText($"{itemView.EquipmentData.Id}");
        rareText.SetText($"{itemView.EquipmentData.Rarity}");
        icon.sprite = itemView.EquipmentData.UIData.Icon;

        baseAttackText.SetText($"{itemView.EquipmentData.AttackValue}");
        enchanceAttackText.SetText($"{itemView.EquipmentData.EnchanceLevels[itemView.EquipmentData.Level]}");
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
        enhanceButton.onClick.AddListener(OnEnhanceButtonClicked);
        enhancePopup.CloseButtonClicked += OnPopupCloseButtonClicked;
    }

    private void Unsubscribe()
    {
        switchButtonsController.FirstButtonClicked -= OnInsertsButtonClicked;
        switchButtonsController.SecondButtonClicked -= OnStatsButtonClicked;
        enhanceButton.onClick.RemoveListener(OnEnhanceButtonClicked);
        enhancePopup.CloseButtonClicked -= OnPopupCloseButtonClicked;
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

    private void OnEnhanceButtonClicked()
    {
        object[] args = new object[] {itemView};
        enhancePopup.Init(args);
        enhancePopup.ForceShow();
    }

    private void OnPopupCloseButtonClicked()
    {
        enhancePopup.ForceHide();
    }
    #endregion

    private void OnDestroy()
    {
        Unsubscribe();
    }
}