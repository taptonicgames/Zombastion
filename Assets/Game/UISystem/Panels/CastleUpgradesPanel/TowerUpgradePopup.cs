using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerUpgradePopup : AbstractPopup
{
    [Space(10), Header("Tower view")]
    [SerializeField] private TMP_Text tittleText;
    [SerializeField] private Image towerIcon;
    [SerializeField] private TMP_Text towerDescription;

    [Space(10), Header("Tower stats")]
    [SerializeField] private TowerStatItemView[] towerStatItemViews;

    [Space(10), Header("Tower skills")]
    [SerializeField] private Transform towerSkillItemViewsContainer;
    [SerializeField] private TowerSkillItemView towerSkillItemViewPrefab;

    [Space(10), Header("Upgrade view")]
    [SerializeField] private TMP_Text upgradeText;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TMP_Text priceText;

    [Space(10), Header("Card info button")]
    [SerializeField] private Button cardInfoButton;

    public event Action Upgraded;

    protected override void Awake()
    {
        base.Awake();

        upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
        cardInfoButton.onClick.AddListener(OnCardInfoButtonClicked);
    }

    public override void Init(object[] args)
    {
        
    }

    private void OnUpgradeButtonClicked()
    {
        Upgraded?.Invoke();
    }

    private void OnCardInfoButtonClicked()
    {
        
    }

    private void OnDestroy()
    {
        upgradeButton.onClick.RemoveListener(OnUpgradeButtonClicked);
        cardInfoButton.onClick.RemoveListener(OnCardInfoButtonClicked);
    }
}