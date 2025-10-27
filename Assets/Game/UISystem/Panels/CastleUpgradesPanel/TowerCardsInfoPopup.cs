using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Zenject;

public class TowerCardsInfoPopup : AbstractPopup
{
    [SerializeField] private TMP_Text tittle;
    [SerializeField] private Transform cardsContainer;

    private BattleUpgradeStorage battleUpgradeStorage;
    private BattleUpgradeConfigsPack upgradeConfigsPack;
    private List<Card> cards = new List<Card>();

    [Inject] private CardsUpgradeManager cardsUpgradeManager;
    [Inject] private SharedObjects sharedObjects;

    public override void Init(object[] args)
    {
        BattleUpgradeType upgradeType = (BattleUpgradeType)args[0];
        WeaponType weaponType = (WeaponType)args[1];
        tittle.SetText($"{(string)args[2]}");
        upgradeConfigsPack = (BattleUpgradeConfigsPack)args[3];
        battleUpgradeStorage = (BattleUpgradeStorage)args[4];

        IEnumerable<BattleUpgradeConfig> configs = cardsUpgradeManager.GetUpgradeConfigsByType(upgradeType);
        var filterByWeaponType = configs.Where(i => i.WeaponType.Equals(weaponType));
        
        HideCards();
        UpdateInfo(filterByWeaponType);
    }

    private void HideCards()
    {
        foreach (var card in cards)
            card.gameObject.SetActive(false);
    }

    private void UpdateInfo(IEnumerable<BattleUpgradeConfig> configs)
    {
        int counter = 0;

        foreach (BattleUpgradeConfig config in configs)
        {
            Card card = null;

            if (cards.Count <= counter)
                card = CreateCard();
            else
                card = cards[counter];

            card.gameObject.SetActive(true);
            card.Init();
            card.Show(config, battleUpgradeStorage, upgradeConfigsPack);
            card.SetInteractable(false);

            counter++;
        }
    }

    private Card CreateCard()
    {
        var card = sharedObjects.InstantiateAndGetObject<Card>(
               Constants.UPGRADE_CARD, cardsContainer);

        cards.Add(card);
        return card;
    }
}