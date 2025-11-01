using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClothItemView : MonoBehaviour
{
    [field: SerializeField] public EquipmentType Type { get; private set; }
    [SerializeField] private Button button;
    [SerializeField] private Transform view;
    [SerializeField] private TMP_Text tittle;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private Image icon;
    [SerializeField] private Image background;
    [SerializeField] private InsertItemView[] insertItems;

    public EquipmentData EquipmentData { get; private set; }

    public event Action<ClothItemView> ItemClicked;

    public void Init(EquipmentData equipmentData)
    {
        EquipmentData = equipmentData;

        tittle.SetText(EquipmentData.UIData.Tittle);
        icon.sprite = EquipmentData.UIData.Icon;
        background.sprite = EquipmentData.UIData.BG;

        button.onClick.AddListener(OnButtonClicked);

        for (int i = 0; i < insertItems.Length; i++)
        {
            insertItems[i].Deactivate();
            insertItems[i].gameObject.SetActive(i < EquipmentData.InsertAvilableAmount);
        }
    }

    public InsertItemView GetEmptyInsertSlot()
    {
        for (int i = 0; i < EquipmentData.InsertDatas.Length; i++)
            if (insertItems[i].InsertData == null)
                return insertItems[i];

        return null;
    }

    public void UpdateDatas()
    {
        for (int i = 0; i < EquipmentData.InsertDatas.Length; i++)
        {
            insertItems[i].gameObject.SetActive(i < EquipmentData.InsertAvilableAmount);

            insertItems[i].ClearData();
            
            if (EquipmentData.InsertDatas[i] != null)
                insertItems[i].SetInsertData(EquipmentData.InsertDatas[i]);
        }
    }

    private void OnButtonClicked()
    {
        ItemClicked?.Invoke(this);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnButtonClicked);
    }
}