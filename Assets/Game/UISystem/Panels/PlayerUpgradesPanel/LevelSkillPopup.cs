using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static SkillsTreeSO;

public class LevelSkillPopup : AbstractPopup
{
    [SerializeField] private TMP_Text tittle;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text valueText;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private Image icon;
    [SerializeField] private Button openButton;
    [SerializeField] private Transform buttonViewObject;
    [SerializeField] private HorizontalLayoutGroup descriptionlayoutGroup;

    private SkillTreeData data;

    public event Action<SkillTreeData> SkillOpened;

    protected override void Awake()
    {
        base.Awake();

        openButton.onClick.AddListener(OnButtonClicked);
    }

    public override void Init(object[] args)
    {
        data = (SkillTreeData)args[0];
        bool isShowButton = (bool)args[1];

        tittle.SetText(data.Tittle);
        description.SetText(data.Description);
        valueText.SetText($"{data.Prefix}{data.Value}{data.Postfix}");

        //TODO: implement currency manager
        bool isAvailable = 0 >= data.Price;

        string color = isAvailable ? "green" : "red";
        priceText.SetText($"<color={color}>{0}</color>/{data.Price}");

        icon.sprite = data.Icon;
        openButton.interactable = isAvailable;

        buttonViewObject.gameObject.SetActive(isShowButton);
    }

    public override void Show(Action callback = null)
    {
        base.Show(() =>
        {
        descriptionlayoutGroup.enabled = false;
        descriptionlayoutGroup.enabled = true;
        });
    }

    private void OnButtonClicked()
    {
        //TODO: implement remove currency

        buttonViewObject.gameObject.SetActive(false);
        SkillOpened?.Invoke(data);
    }

    private void OnDestroy()
    {
        openButton.onClick.RemoveListener(OnButtonClicked);
    }
}