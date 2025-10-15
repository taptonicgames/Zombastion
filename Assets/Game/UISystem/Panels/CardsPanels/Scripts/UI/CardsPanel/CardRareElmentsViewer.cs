using Coffee.UIExtensions;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardRareElmentsViewer : MonoBehaviour
{
    [SerializeField] private TMP_Text _rareTittleText;
    [SerializeField] private UIGradient _cardGradient;
    [SerializeField] private Transform _starsView;
    [SerializeField] private Color _fadeColor;
    [SerializeField] private Image[] _stars;

    [Space(10)]
    [Header("Animation picked card")]
    [SerializeField] private UIParticle _uiParticle;
    [SerializeField] private Vector3 _targetScale = new Vector3(1.25f, 1.25f, 1.25f);
    [SerializeField] private float _duration = 0.25f;

    private int _currentCount;
    private Sequence _sequence;

    public void ShowStarsIndicator(BattleUpgradeConfig battleUpgrade, BattleUpgradeStorage battleUpgradeStorage, BattleUpgradeConfigsPack upgradeConfigsPack)
    {
        _rareTittleText.SetText($"{battleUpgrade.RareType}");

        SetGradient(battleUpgrade.RareType, upgradeConfigsPack);

        _uiParticle.gameObject.SetActive(false);
        _currentCount = GetCount(battleUpgrade, battleUpgradeStorage);

        _starsView.gameObject.SetActive(battleUpgrade.MaxPickedCount > 0);

        for (int i = 0; i < _stars.Length; i++)
        {
            _stars[i].gameObject.SetActive(i < battleUpgrade.MaxPickedCount);

            if (_stars[i].gameObject.activeSelf)
                _stars[i].color = i < _currentCount ? Color.white : _fadeColor;
        }
    }

    public void OnCardReached(Action callback)
    {
        _currentCount++;
        _stars[_currentCount - 1].color = Color.white;
        _uiParticle.transform.position = _stars[_currentCount - 1].transform.position;

        _sequence.Kill();
        _sequence = DOTween.Sequence();
        _sequence.SetUpdate(true);
        _sequence.Append(_stars[_currentCount - 1].transform.DOScale(_targetScale, _duration).From(Vector3.one).SetEase(Ease.Linear));
        _sequence.AppendCallback(() => _uiParticle.gameObject.SetActive(true));
        _sequence.Append(_stars[_currentCount - 1].transform.DOScale(Vector3.one, _duration).SetEase(Ease.Linear));
        _sequence.OnComplete(() => callback?.Invoke());
    }

    private void SetGradient(BattleUpgradeRareType type, BattleUpgradeConfigsPack upgradeConfigsPack)
    {
        switch (type)
        {
            case BattleUpgradeRareType.Common:
                _cardGradient.SetGradientColors(upgradeConfigsPack.CommonGradientStartColor, upgradeConfigsPack.CommonGradientEndColor);
                break;
            case BattleUpgradeRareType.Rare:
                _cardGradient.SetGradientColors(upgradeConfigsPack.RareGradientStartColor, upgradeConfigsPack.RareGradientEndColor);
                break;
            case BattleUpgradeRareType.Epic:
                _cardGradient.SetGradientColors(upgradeConfigsPack.EpicGradientStartColor, upgradeConfigsPack.EpicGradientEndColor);
                break;
            case BattleUpgradeRareType.Legend:
                _cardGradient.SetGradientColors(upgradeConfigsPack.LegendGradientStartColor, upgradeConfigsPack.LegendGradientEndColor);
                break;
        }

        _rareTittleText.color = _cardGradient.color1;
    }

    private int GetCount(BattleUpgradeConfig battleUpgrade, BattleUpgradeStorage battleUpgradeStorage)
    {
        return battleUpgradeStorage.Upgrades[battleUpgrade.UpgradeType];
    }
}