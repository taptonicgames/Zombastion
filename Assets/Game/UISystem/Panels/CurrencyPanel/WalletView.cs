using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class WalletView : MonoBehaviour
{
    [SerializeField] private Image currencyIcon;
    [SerializeField] private TMP_Text currencyText;
    [SerializeField] private Button button;

    [Space(10), Header("Cheat currency")]
    [SerializeField] private int cheatCurrency;
    
    public CurrencyType Type { get; private set; }

    [Inject] private CurrencyManager currencyManager;

    private void Awake()
    {
        button.onClick.AddListener(OnButtonClicked);
        currencyManager.CurrencyChanged += OnCurrencyChanged;
    }

    public void Init(CurrencyType currencyType)
    {
        Type = currencyType;

        UpdateInfo();
    }

    private void UpdateInfo()
    {
        currencyIcon.sprite = currencyManager.GetCurrencyData(Type).UIData.Icon;

        string text = 
            Type == CurrencyType.Energy ?
            $"{currencyManager.GetCurrencyAmount(Type)}/{currencyManager.GetMaxEnergyAmount()}" :
            $"{currencyManager.GetCurrencyAmount(Type)}";

        currencyText.SetText(text);
    }

    private void OnButtonClicked()
    {
        //TODO: implement monetization
        currencyManager.AddCurrency(Type, cheatCurrency);
        
        UpdateInfo();
    }

    private void OnCurrencyChanged(CurrencyType type)
    {
        if (Type == type)
            UpdateInfo();
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnButtonClicked);
        currencyManager.CurrencyChanged -= OnCurrencyChanged;
    }
}