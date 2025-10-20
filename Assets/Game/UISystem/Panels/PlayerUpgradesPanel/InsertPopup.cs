using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InsertPopup : AbstractPopup
{
    [SerializeField] private Button embedButton;
    [SerializeField] private BagInsertButton insertButton;

    [Space(10), Header("View")]
    [SerializeField] private TMP_Text rareText;
    [SerializeField] private TMP_Text tittleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private InsertStatsItem[] insertStatsItems;

    private InsertData insertData;
    private EquipmentData equipmentData;

    public event Action<InsertData> ButtonClicked;

    protected override void Awake()
    {
        base.Awake();

        embedButton.onClick.AddListener(OnButtonClicked);
    }

    public override void Init(object[] args)
    {
        insertData = (InsertData)args[0];
        equipmentData = (EquipmentData)args[1];

        insertButton.Init(insertData);
        rareText.SetText($"{insertData.Rarity}");
        tittleText.SetText($"{insertData.Type}-{insertData.Rarity}Insert");

        for (int i = 0; i < insertStatsItems.Length; i++)
            insertStatsItems[i].Init(equipmentData.InsertDatas[i]);
    }

    private void OnButtonClicked()
    {
        ButtonClicked?.Invoke(insertData);
    }

    private void OnDestroy()
    {
        embedButton.onClick.RemoveListener(OnButtonClicked);
    }
}