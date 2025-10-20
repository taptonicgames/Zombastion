using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BagInsertButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text countText;
    [SerializeField] private Image background;
    [SerializeField] private Image icon;
    [SerializeField] private Image pickedIcon;
    [SerializeField] private Image equipmentTypeIcon;

    public int Count { get; private set; } = 1;
    public InsertData InsertData { get; private set; }

    public event Action<BagInsertButton> ButtonClicked;

    private void Awake()
    {
        button.onClick.AddListener(OnButtonClicked);
    }

    public void Init(InsertData insertData)
    {
        InsertData = insertData;

        icon.sprite = InsertData.RarityUIData.Icon;
        background.sprite = InsertData.RarityUIData.BG;
        equipmentTypeIcon.sprite = InsertData.EquipUIData.Icon;
    }

    public void UpdateCount(int value)
    {
        Count = value;
        countText.SetText($"{Count}");
    }

    public void ChangePickedState(bool isPicked)
    {
        pickedIcon.gameObject.SetActive(isPicked);
        button.interactable = !isPicked;
    }

    private void OnButtonClicked()
    {
        ButtonClicked?.Invoke(this);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnButtonClicked);
    }
}