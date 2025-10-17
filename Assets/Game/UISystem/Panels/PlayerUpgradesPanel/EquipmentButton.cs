using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Sprite pickedSprite;
    [SerializeField] private Sprite notPickedSprite;
    [SerializeField] private Image icon;
    [SerializeField] private Image background;
    [SerializeField] private TMP_Text levelText;

    public EquipmentData Data { get; private set; }

    public event Action<EquipmentButton> ButtonClicked;

    public void Init(EquipmentData data)
    {
        Data = data;

        UpdateInfo();

        button.onClick.AddListener(OnButtonClicked);
    }

    public void ChangePickedState(bool isPicked)
    {
        background.sprite = isPicked ? pickedSprite : notPickedSprite;
        button.interactable = !isPicked;
    }

    public void UpdateInfo()
    {
        icon.sprite = Data.UIData.Icon;
        levelText.SetText($"Lv. {Data.Level}");
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