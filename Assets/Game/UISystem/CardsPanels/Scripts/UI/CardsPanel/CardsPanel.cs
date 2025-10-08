using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardsPanel : BasePanel
{
    [SerializeField] private Transform _tittle;
    [SerializeField] private Card[] _cards;
    [SerializeField] private Image _fadeImage;

    [Space(20)]
    [SerializeField] private GridLayoutGroup _gridLayoutGroup;
    [SerializeField] private ContentSizeFitter _contentSizeFitter;

    private BattleUpgradeStorage _battleUpgradeStorage;
    private BattleUpgradeConfigsPack _upgradeConfigsPack;
    private List<BattleUpgradeConfig> _upgrades = new List<BattleUpgradeConfig>();
    private Sequence _sequence;
    private float _duration = 0.25f;

    public override PanelType PanelType => PanelType.BattleUpgrade;

    public event Action PickCardsCompleted;

    public void Init(BattleUpgradeConfigsPack upgradeConfigsPack, BattleUpgradeHandler battleUpgradeHandler,
        BattleUpgradeStorage battleUpgradeStorage)
    {
        _upgradeConfigsPack = upgradeConfigsPack;
        _battleUpgradeStorage = battleUpgradeStorage;

        for (int i = 0; i < _cards.Length; i++)
        {
            _cards[i].Init();
            _cards[i].ButtonClicked += OnCardPicked;
        }
    }

    #region Card picked
    private void OnCardPicked(Card card)
    {
        foreach (var item in _cards)
        {
            item.SetInteractable(false);

            if (item == card)
                _upgrades.Add(card.CurrentUpgrade);
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
                cards[i].OnCardPicked(() =>
                {
                    EndAnimation();
                }, delay * (i + 1));
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


        for (int i = 0; i < _cards.Length; i++)
            _cards[i].gameObject.SetActive(false);

        _gridLayoutGroup.enabled = true;
        _contentSizeFitter.enabled = true;
    }
    #endregion

    public override void Show()
    {
        base.Show();

        if (_fadeImage != null)
        {
            Color fadeColor = _fadeImage.color;
            fadeColor.a = 0f;
            _fadeImage.color = fadeColor;
        }

        for (int i = 0; i < _cards.Length; i++)
        {
            var upgrade = _battleUpgradeStorage.GetBattleUpgrade();
            _cards[i].gameObject.SetActive(true);
            _cards[i].Show(upgrade, _battleUpgradeStorage, _upgradeConfigsPack);
        }

        AnimateShow();
    }

    public override void Hide()
    {
        base.Hide();
        _battleUpgradeStorage.ResetList();
    }

    public void ResetUpgrades()
    {
        _upgrades.Clear();
    }

    public List<BattleUpgradeConfig> GetUpgrades()
    {
        return _upgrades;
    }

    private void AnimateShow()
    {
        _tittle.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

        for (int i = 0; i < _cards.Length; i++)
        {
            _cards[i].transform.localScale = Vector3.zero;
        }

        _sequence.Kill();
        _sequence = DOTween.Sequence();
        _sequence.SetUpdate(true);

        if (_fadeImage != null)
        {
            _sequence.Join(_fadeImage.DOFade(0.5f, _duration).SetEase(Ease.InOutQuad));
        }

        _sequence.Append(_tittle.transform.DOScale(Vector3.one, _duration).SetEase(Ease.OutBack));

        float cardDelay = 0.1f;

        for (int i = 0; i < _cards.Length; i++)
        {
            float startDelay = cardDelay * i;

            int cardIndex = i;

            _sequence.InsertCallback(_duration + startDelay, () =>
            {
                _cards[cardIndex].transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            });

            _sequence.Insert(_duration + startDelay, _cards[i].transform.DOScale(Vector3.one, _duration).SetEase(Ease.OutBack));
        }

        float totalDuration = _duration + (_cards.Length - 1) * cardDelay + _duration;

        _sequence.OnComplete(() =>
        {
            for (int i = 0; i < _cards.Length; i++)
                _cards[i].SetInteractable(true);
        });
    }

    private void OnDestroy()
    {
        for (int i = 0; i < _cards.Length; i++)
            _cards[i].ButtonClicked -= OnCardPicked;
    }
}