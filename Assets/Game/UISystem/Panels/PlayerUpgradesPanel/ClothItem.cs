using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClothItem : MonoBehaviour
{
    [field: SerializeField] public ClothItemSO SOData { get; private set; }
    [field: SerializeField] public ClothType Type { get; private set; }
    [SerializeField] private Button button;
    [SerializeField] private Transform view;
    [SerializeField] private TMP_Text tittle;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private Image icon;
    [SerializeField] private InsertItem[] insertItems;

    public event Action<ClothItem> ItemClicked;

    public void Init()
    {
        tittle.SetText(SOData.Tittle);
        icon.sprite = SOData.Icon;

        button.onClick.AddListener(OnButtonClicked);

        for (int i = 0; i < insertItems.Length; i++)
            insertItems[i].Deactivate();
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