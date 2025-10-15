using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BagClothPopup : AbstractPopup
{
    [Space(10), Header("Current cloth item")]
    [SerializeField] private BagEquipmentButton currentEquipButton;
    [SerializeField] private TMP_Text currentEquipTittleText;
    [SerializeField] private TMP_Text currentEquipClothTypeText;
    [SerializeField] private TMP_Text currentEquipStatText;

    [Space(10), Header("Target cloth item")]
    [SerializeField] private BagEquipmentButton targetEquipButton;
    [SerializeField] private TMP_Text targetEquipTittleText;
    [SerializeField] private TMP_Text targetEquipClothTypeText;
    [SerializeField] private TMP_Text targetEquipStatText;

    [Space(10), Header("Buttons")]
    [SerializeField] private Transform buttonsView;
    [SerializeField] private Button decomposeButton;
    [SerializeField] private Button replaceButton;

    private EquipmentData currentData;
    private EquipmentData targetData;

    public event Action<EquipmentData> ClothReplaced;

    protected override void Awake()
    {
        base.Awake();

        decomposeButton.onClick.AddListener(OnDecomposeButtonClicked);
        replaceButton.onClick.AddListener(OnReplaceButtonClicked);
    }

    public override void Init(object[] args)
    {
        currentData = (EquipmentData)args[0];
        targetData = (EquipmentData)args[1];

        UpdateInfo();
    }

    private void UpdateInfo()
    {
        currentEquipButton.Init(currentData);
        currentEquipTittleText.SetText($"{currentData.UIData.Tittle}");
        currentEquipClothTypeText.SetText($"{currentData.Type}");
        currentEquipStatText.SetText($"{currentData.AttackValue}");

        targetEquipButton.Init(targetData);
        targetEquipTittleText.SetText($"{targetData.UIData.Tittle}");
        targetEquipClothTypeText.SetText($"{targetData.Type}");
        targetEquipStatText.SetText($"{targetData.AttackValue}");

        Debug.LogWarning($"{currentData.Id} == {targetData.Id}");
        buttonsView.gameObject.SetActive(currentData.Id != targetData.Id);
    }

    private void OnDecomposeButtonClicked()
    {
        //TODO: open decompose popup
    }

    private void OnReplaceButtonClicked()
    {
        ClothReplaced?.Invoke(targetData);
    }

    private void OnDestroy()
    {
        decomposeButton.onClick.RemoveListener(OnDecomposeButtonClicked);
        replaceButton.onClick.RemoveListener(OnReplaceButtonClicked);
    }
}
