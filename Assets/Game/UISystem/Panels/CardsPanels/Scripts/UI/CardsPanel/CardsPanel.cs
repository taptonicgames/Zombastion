using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CardsPanel : AbstractPanel
{
    [Inject]
    private readonly SharedObjects sharedObjects;

    [Inject]
    private BattleUIManager battleUIManager;

    [SerializeField]
    private Transform _tittle;

    [SerializeField]
    private Transform container;
    private BattleUpgradeConfig[] upgradeConfigs;
    private List<Card> cards = new();

    [SerializeField]
    private Image _fadeImage;

    [Space(20)]
    [SerializeField]
    private GridLayoutGroup _gridLayoutGroup;

    [SerializeField]
    private ContentSizeFitter _contentSizeFitter;

    private BattleUpgradeStorage battleUpgradeStorage;
    private BattleUpgradeConfigsPack upgradeConfigsPack;
    private List<BattleUpgradeConfig> upgrades = new List<BattleUpgradeConfig>();
    private Sequence sequence;
    private float duration = 0.25f;

    public override PanelType Type => PanelType.BattleUpgrade;

    public event Action PickCardsCompleted;

    public override void Init(object[] arr)
    {
        if (arr == null) return;
        upgradeConfigsPack = (BattleUpgradeConfigsPack)arr[0];
        battleUpgradeStorage = (BattleUpgradeStorage)arr[1];
        upgradeConfigs = (BattleUpgradeConfig[])arr[2];
        CreateCard();

        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].Init();
            cards[i].ButtonClicked += OnCardPicked;
        }
    }

    private void CreateCard()
    {
        foreach (var config in upgradeConfigs)
        {
            var card = sharedObjects.InstantiateAndGetObject<Card>(
                Constants.UPGRADE_CARD,
                container
            );

            card.Init();
            card.Show(config, battleUpgradeStorage, upgradeConfigsPack);
            cards.Add(card);
        }
    }

    public void Init(
        BattleUpgradeConfigsPack upgradeConfigsPack,
        BattleUpgradeHandler battleUpgradeHandler,
        BattleUpgradeStorage battleUpgradeStorage
    )
    {
        this.upgradeConfigsPack = upgradeConfigsPack;
        this.battleUpgradeStorage = battleUpgradeStorage;
    }

    #region Card picked
    private void OnCardPicked(Card card)
    {
        foreach (var item in this.cards)
        {
            item.SetInteractable(false);

            if (item == card)
                upgrades.Add(card.CurrentUpgrade);
        }

        List<Card> cards = new List<Card>();
        cards.Add(card);
        AnimateCards(cards);
    }

    private void AnimateCards(List<Card> cards, float delay = 0.0f)
    {
        _gridLayoutGroup.enabled = false;
        _contentSizeFitter.enabled = false;

        for (int i = 0; i < cards.Count; i++)
        {
            if (i == cards.Count - 1)
            {
                cards[i]
                    .OnCardPicked(
                        () =>
                        {
                            EndAnimation();
                        },
                        delay * (i + 1)
                    );
            }
            else
            {
                cards[i].OnCardPicked(null, delay * (i + 1));
            }
        }
    }

    private void EndAnimation()
    {
        PickCardsCompleted?.Invoke();

        EventBus<UpgradeChoosenEvnt>.Publish(
            new() { type = upgradeConfigs.First().UpgradeType, config = upgradeConfigs.First() }
        );

        for (int i = 0; i < cards.Count; i++)
            cards[i].gameObject.SetActive(false);

        _gridLayoutGroup.enabled = true;
        _contentSizeFitter.enabled = true;
        Hide();
    }
    #endregion

    //public void Show()
    //{
    //    base.Show();

    //    if (_fadeImage != null)
    //    {
    //        Color fadeColor = _fadeImage.color;
    //        fadeColor.a = 0f;
    //        _fadeImage.color = fadeColor;
    //    }

    //    for (int i = 0; i < _cards.Length; i++)
    //    {
    //        var upgrade = _battleUpgradeStorage.GetBattleUpgrade();
    //        _cards[i].gameObject.SetActive(true);
    //        _cards[i].Show(upgrade, _battleUpgradeStorage, _upgradeConfigsPack);
    //    }

    //    AnimateShow();
    //}

    //public void Hide()
    //{
    //    base.Hide();
    //    _battleUpgradeStorage.ResetList();
    //}

    public void ResetUpgrades()
    {
        upgrades.Clear();
        battleUIManager.ClearContainer(container);
        cards.Clear();
    }

    public List<BattleUpgradeConfig> GetUpgrades()
    {
        return upgrades;
    }

    public override UniTask OnShow()
    {
        AnimateShow();
        return base.OnShow();
    }

    public override UniTask OnHide()
    {
        if (battleUpgradeStorage != null)
            battleUpgradeStorage.ResetList();

        ResetUpgrades();
        return base.OnHide();
    }

    private void AnimateShow()
    {
        _tittle.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].transform.localScale = Vector3.zero;
        }

        sequence.Kill();
        sequence = DOTween.Sequence();
        sequence.SetUpdate(true);

        if (_fadeImage != null)
        {
            sequence.Join(_fadeImage.DOFade(0.5f, duration).SetEase(Ease.InOutQuad));
        }

        sequence.Append(_tittle.transform.DOScale(Vector3.one, duration).SetEase(Ease.OutBack));

        float cardDelay = 0.1f;

        for (int i = 0; i < cards.Count; i++)
        {
            float startDelay = cardDelay * i;

            int cardIndex = i;

            sequence.InsertCallback(
                duration + startDelay,
                () =>
                {
                    cards[cardIndex].transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                }
            );

            sequence.Insert(
                duration + startDelay,
                cards[i].transform.DOScale(Vector3.one, duration).SetEase(Ease.OutBack)
            );
        }

        float totalDuration = duration + (cards.Count - 1) * cardDelay + duration;

        sequence.OnComplete(() =>
        {
            for (int i = 0; i < cards.Count; i++)
                cards[i].SetInteractable(true);
        });
    }

    private void OnDestroy()
    {
        for (int i = 0; i < cards.Count; i++)
            cards[i].ButtonClicked -= OnCardPicked;
    }
}
