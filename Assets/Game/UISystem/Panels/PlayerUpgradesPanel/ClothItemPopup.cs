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
    [SerializeField] private TMP_Text baseAttackDescription;
    [SerializeField] private TMP_Text baseAttackText;
    [SerializeField] private TMP_Text enchanceAttackText;

    private ClothItemView itemView;
    private CurrencyManager currencyManager;
    private UpgradesManager upgradesManager;
    private EquipmentManager equipmentManager;

    public event Action<InsertData> InsertDataRemoved;

    protected override void Awake()
    {
        base.Awake();

        switchButtonsController.Init();

        Subscribe();
    }

    public override void Init(object[] args)
    {
        itemView = (ClothItemView)args[0];
        currencyManager = (CurrencyManager)args[1];
        upgradesManager = (UpgradesManager)args[2];
        equipmentManager = (EquipmentManager)args[3];

        UpdateInfo();
    }

    public override void Show(Action callback = null)
    {
        base.Show(callback);

        switchButtonsController.Show();
    }

    private void UpdateInfo()
    {
        tittle.SetText($"{itemView.EquipmentData.Id}");
        rareText.SetText($"{itemView.EquipmentData.Rarity}");
        icon.sprite = itemView.EquipmentData.UIData.Icon;

        baseAttackDescription.SetText($"{equipmentManager.GetStatName(itemView.EquipmentData.Type)}");
        baseAttackText.SetText($"{itemView.EquipmentData.Value}");
        enchanceAttackText.SetText($"{itemView.EquipmentData.EnchanceLevels[itemView.EquipmentData.Level]}");

        if (insertStatsItems.Length != itemView.EquipmentData.InsertDatas.Length)
            throw new ArgumentException($"popup items not equal cloth stat items!");

        for (int i = 0; i < insertStatsItems.Length; i++)
        {
            insertStatsItems[i].Init(itemView.EquipmentData.InsertDatas[i]);
            insertStatsItems[i].gameObject.SetActive(i < itemView.EquipmentData.InsertAvilableAmount);
        }
    }

    #region Events
    private void Subscribe()
    {
        switchButtonsController.FirstButtonClicked += OnInsertsButtonClicked;
        switchButtonsController.SecondButtonClicked += OnStatsButtonClicked;
        enhanceButton.onClick.AddListener(OnEnhanceButtonClicked);
        enhancePopup.CloseButtonClicked += OnPopupCloseButtonClicked;

        for (int i = 0; i < insertStatsItems.Length; i++)
            insertStatsItems[i].ButtonClicked += OnInsertRemoved;
    }

    private void Unsubscribe()
    {
        switchButtonsController.FirstButtonClicked -= OnInsertsButtonClicked;
        switchButtonsController.SecondButtonClicked -= OnStatsButtonClicked;
        enhanceButton.onClick.RemoveListener(OnEnhanceButtonClicked);
        enhancePopup.CloseButtonClicked -= OnPopupCloseButtonClicked;

        for (int i = 0; i < insertStatsItems.Length; i++)
            insertStatsItems[i].ButtonClicked -= OnInsertRemoved;
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
        object[] args = new object[] 
        {
            itemView,
            currencyManager,
            upgradesManager
        };

        enhancePopup.Init(args);
        enhancePopup.ForceShow();
    }

    private void OnPopupCloseButtonClicked()
    {
        enhancePopup.ForceHide();
        UpdateInfo();
    }

    private void OnInsertRemoved(InsertStatsItem item)
    {
        InsertData insertData = item.InsertData;
        itemView.EquipmentData.RemoveInsert(insertData);
        item.ChangeActiveState(false);
        InsertDataRemoved?.Invoke(insertData);
    }
    #endregion

    private void OnDestroy()
    {
        Unsubscribe();
    }
}