using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InsertStatsItem : MonoBehaviour
{
    [SerializeField] private BagInsertButton bagInsertButton;
    [SerializeField] private TMP_Text description;
    [SerializeField] private Button removeButton;

    [Header("States")]
    [SerializeField] private RectTransform activateState;
    [SerializeField] private RectTransform deactivateState;

    public InsertData InsertData { get; private set; }

    public event Action<InsertStatsItem> ButtonClicked;

    private void Awake()
    {
        removeButton.onClick.AddListener(OnButtonClicked);
    }

    public void ChangeActiveState(bool isActive)
    {
        activateState.gameObject.SetActive(isActive);
        deactivateState.gameObject.SetActive(!isActive);
    }

    public void Init(InsertData insertData)
    {
        ChangeActiveState(insertData != null);

        if (activateState.gameObject.activeSelf)
        {
            InsertData = insertData;
            bagInsertButton.Init(insertData);
            description.SetText($"{insertData.Id}");
        }
    }
    
    private void OnButtonClicked()
    {
        ButtonClicked?.Invoke(this);
    }

    private void OnDestroy()
    {
        removeButton.onClick.RemoveListener(OnButtonClicked);
    }
}