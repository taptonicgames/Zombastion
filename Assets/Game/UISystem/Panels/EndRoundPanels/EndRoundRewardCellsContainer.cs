using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine;
using Zenject;

public class EndRoundRewardCellsContainer : MonoBehaviour
{
    [SerializeField] private EndRoundRewadCell[] cells;
    [SerializeField] private float animateDurationPerCell;

    private int amountShowCells;
    private Sequence sequence;

    [Inject] private CurrencyManager currencyManager;

    public void Init(RewardData[] rewards)
    {
        amountShowCells = rewards.Length;

        for (int i = 0; i < cells.Length; i++)
        {
            if (i < rewards.Length)
            {
                CurrencyData currencyData = currencyManager.GetCurrencyData(rewards[i].CurrencyType);
                cells[i].Init(currencyData.UIData.Icon, currencyData.UIData.BG, rewards[i].Value);
            }
            cells[i].View.localScale = Vector3.zero;
        }
    }

    public async UniTask AnimateCells()
    {
        bool isAnimate = true;

        sequence.Kill();
        sequence = DOTween.Sequence();
        sequence.SetUpdate(true);

        for (int i = 0; i < cells.Length; i++)
        {
            if (i < amountShowCells)
                sequence.Append(cells[i].View.transform.DOScale(Vector3.one, animateDurationPerCell).SetEase(Ease.OutBack));
        }

        sequence.OnComplete(() => isAnimate = false);

        while (isAnimate)
            await UniTask.Yield();
    }

    public void IncreaseReward(float modifier)
    {
        for (int i = 0; i < amountShowCells; i++)
            cells[i].IncreaseReward(modifier);
    }
}