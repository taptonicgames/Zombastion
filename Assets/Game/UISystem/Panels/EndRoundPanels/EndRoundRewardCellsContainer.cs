using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EndRoundRewardCellsContainer : MonoBehaviour
{
    [SerializeField] private EndRoundRewadCell[] cells;
    [SerializeField] private float animateDurationPerCell;

    private int amountShowCells;
    private Sequence sequence;

    [Inject] private SpritesManager spritesManager;

    public void Init(RewardData rewardsData)
    {
        List<AbstractRewardData> rewards = rewardsData.GetRewardDatas();
        amountShowCells = rewards.Count;

        for (int i = 0; i < cells.Length; i++)
        {
            if (i < rewards.Count)
            {
                Sprite icon = null;
                Sprite bg = null;
                spritesManager.UpdateSprites(rewards[i], ref icon, ref bg);
                cells[i].Init(icon, bg, rewards[i].Value);
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