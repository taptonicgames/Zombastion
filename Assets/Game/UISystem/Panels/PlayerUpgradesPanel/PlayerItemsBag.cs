using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerItemsBag : MonoBehaviour
{
    [SerializeField] private SwitchButtonsController switchButtonsController;
    [SerializeField] private BagEquipmentButton bagEquipmentButtonPrefab;
    [SerializeField] private BagInsertButton bagInsertButtonPrefab;
    [SerializeField] private InsertPopup insertPopup;
    [SerializeField] private BagClothPopup bagClothPopup;

    [Space(10), Header("Bag Items containers")]
    [SerializeField] private Transform clothItemsBagScrollView;
    [SerializeField] private Transform clothItemsBagContainer;

    [SerializeField] private Transform insertsScrollView;
    [SerializeField] private Transform insertsContainer;

    private EquipmentManager equipmentManager;
    private List<BagInsertButton> bagInsertButtons = new List<BagInsertButton>();
    private List<BagEquipmentButton> bagEquipmentButtons = new List<BagEquipmentButton>();

    public event Action<BagInsertButton> InsertEmbed;
    public event Action<BagEquipmentButton> ClothReplaced;

    public void Init(EquipmentManager equipmentManager)
    {
        this.equipmentManager = equipmentManager;

        switchButtonsController.Init();

        InitClothes();
        InitInserts();
        Subscribe();

        insertPopup.ForceHide();
        bagClothPopup.ForceHide();
    }

    public void Show()
    {
        switchButtonsController.Show();
    }

    public void ShowInsert(InsertData insertData)
    {
        foreach (var button in bagInsertButtons)
        {
            if (button.gameObject.activeSelf == false && button.InsertData.Id == insertData.Id)
                button.gameObject.SetActive(true);
        }
    }

    private void InitClothes()
    {
        List<EquipmentData> equipmentDatas = equipmentManager.GetClothDatas();

        foreach (var data in equipmentDatas)
        {
            if (HasEquipmentDataAtList(data))
            {
                BagEquipmentButton equipmentButton = bagEquipmentButtons.FirstOrDefault(b => b.EquipmentData.Id == data.Id);
                equipmentButton.UpdateCount(equipmentButton.Count + 1);
            }
            else
            {
                BagEquipmentButton equipmentButton = Instantiate(bagEquipmentButtonPrefab, clothItemsBagContainer);
                equipmentButton.Init(data);
                bagEquipmentButtons.Add(equipmentButton);
            }
        }
    }

    private bool HasEquipmentDataAtList(EquipmentData data)
    {
        foreach (var button in bagEquipmentButtons)
            if (button.EquipmentData.Id == data.Id)
                return true;

        return false;
    }

    private void InitInserts()
    {
        List<InsertData> insertDatas = equipmentManager.GetInsertDatas();

        foreach (var data in insertDatas)
        {
            if (HasInsertDataAtList(data))
            {
                BagInsertButton insertButton = bagInsertButtons.FirstOrDefault(b => b.InsertData.Id == data.Id);
                insertButton.UpdateCount(insertButton.Count + 1);
            }
            else
            {
                BagInsertButton insertButton = Instantiate(bagInsertButtonPrefab, insertsContainer);
                insertButton.Init(data);
                bagInsertButtons.Add(insertButton);
            }
        }
    }

    private bool HasInsertDataAtList(InsertData data)
    {
        foreach (var button in bagInsertButtons)
            if (button.InsertData.Id == data.Id)
                return true;

        return false;
    }

    #region Events
    private void Subscribe()
    {
        switchButtonsController.FirstButtonClicked += OnClothItemsBagButtonClicked;
        switchButtonsController.SecondButtonClicked += OnInsertItemsBagButtonClicked;
        insertPopup.CloseButtonClicked += OnPopupClosed;
        insertPopup.ButtonClicked += OnEmbedButtonClicked;

        foreach (var item in bagInsertButtons)
            item.ButtonClicked += OnInsertClicked;

        bagClothPopup.CloseButtonClicked += OnClothPopupClosed;
        bagClothPopup.ClothReplaced += OnClothReplaced;

        foreach (var item in bagEquipmentButtons)
            item.ButtonClicked += OnClothClicked;
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

    private void OnPopupClosed()
    {
        foreach (var item in bagInsertButtons)
            item.ChangePickedState(false);
        insertPopup.Hide();
    }

    private void OnEmbedButtonClicked(InsertData data)
    {
        foreach (var item in bagInsertButtons)
            item.ChangePickedState(false);
        insertPopup.ForceHide();

        BagInsertButton insertButton = bagInsertButtons.First(d => d.InsertData.Id == data.Id);
        InsertEmbed?.Invoke(insertButton);
        insertButton.gameObject.SetActive(false);
    }

    private void OnInsertClicked(BagInsertButton button)
    {
        foreach (var item in bagInsertButtons)
            item.ChangePickedState(item == button);

        object[] args = new object[] { button.InsertData, equipmentManager.GetEquipmentDataByType(button.InsertData.Type) };
        insertPopup.Init(args);
        insertPopup.Show();
    }

    private void OnClothPopupClosed()
    {
        foreach (var item in bagEquipmentButtons)
            item.ChangePickedState(false);
        bagClothPopup.Hide();
    }

    private void OnClothReplaced(EquipmentData data)
    {
        foreach (var item in bagEquipmentButtons)
            item.ChangePickedState(false);
        bagClothPopup.ForceHide();

        BagEquipmentButton equipButton = bagEquipmentButtons.First(d => d.EquipmentData.Id == data.Id);
        ClothReplaced?.Invoke(equipButton);
        //equipButton.gameObject.SetActive(false);
    }

    private void OnClothClicked(BagEquipmentButton button)
    {
        foreach (var item in bagEquipmentButtons)
            item.ChangePickedState(item == button);

        object[] args = new object[] { equipmentManager.GetEquipmentDataByType(button.EquipmentData.Type) ,button.EquipmentData };
        bagClothPopup.Init(args);
        bagClothPopup.Show();
    }

    private void Unsubscribe()
    {
        switchButtonsController.FirstButtonClicked -= OnClothItemsBagButtonClicked;
        switchButtonsController.SecondButtonClicked -= OnInsertItemsBagButtonClicked;

        insertPopup.CloseButtonClicked -= OnPopupClosed;
        insertPopup.ButtonClicked -= OnEmbedButtonClicked;

        foreach (var item in bagInsertButtons)
            item.ButtonClicked -= OnInsertClicked;

        bagClothPopup.CloseButtonClicked -= OnClothPopupClosed;
        bagClothPopup.ClothReplaced -= OnClothReplaced;

        foreach (var item in bagEquipmentButtons)
            item.ButtonClicked -= OnClothClicked;
    }
    #endregion

    private void OnDestroy()
    {
        Unsubscribe();
    }
}