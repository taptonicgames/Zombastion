using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BagEquipmentButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text tittleText;
    [SerializeField] private TMP_Text countText;
    [SerializeField] private Image icon;
    [SerializeField] private Image background;
    [SerializeField] private Image pickedIcon;

    public EquipmentData EquipmentData { get; private set; }
    public int Count { get; private set; } = 1;

    public event Action<BagEquipmentButton> ButtonClicked;

    private void Awake()
    {
        button.onClick.AddListener(OnButtonClicked);
    }

    public void Init(EquipmentData data)
    {
        EquipmentData = data;
        tittleText.SetText($"{EquipmentData.UIData.Tittle}");
        icon.sprite = EquipmentData.UIData.Icon;
        background.sprite = EquipmentData.UIData.BG;
        pickedIcon.gameObject.SetActive(false);
    }

    public void UpdateCount(int value)
    {
        Count += value;
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
