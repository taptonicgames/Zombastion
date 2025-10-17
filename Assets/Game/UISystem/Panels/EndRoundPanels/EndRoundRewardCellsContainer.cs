using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine;

public class EndRoundRewardCellsContainer : MonoBehaviour
{
    [SerializeField] private EndRoundRewadCell[] cells;
    [SerializeField] private float animateDurationPerCell;

    private Sequence sequence;

    public void Init()
    {
        for (int i = 0; i < cells.Length; i++) 
            cells[i].View.localScale = Vector3.zero;
    }

    public void Show()
    {
        //var rewards = rewardManager.GetRewards();

        //for (var i = 0; i < cells.Length; i++)
        //{
        //    if (i < rewards.Length)
        //    {
        //        var reward = rewards[i];
        //        cells[i].gameObject.SetActive(true);
        //        cells[i].Show(_roundRewardConfig.GetSpriteReward(reward.TypeResource));
        //        cells[i].SetTextValue(reward.Value, i, "x");

        //        if (_selectedSkillsManager.IsSkillSelected(SkillType.HandOfMidas)) cells[i].EnabledEffect(true);
        //        else cells[i].EnabledEffect(false);
        //    }
        //    else
        //    {
        //        cells[i].gameObject.SetActive(false);
        //    }
        //}
    }

    public async UniTask AnimateCells()
    {
        bool isAnimate = true;

        sequence.Kill();
        sequence = DOTween.Sequence();

        for (int i = 0; i < cells.Length; i++)
            sequence.Append(cells[i].View.transform.DOScale(Vector3.one, animateDurationPerCell).SetEase(Ease.OutBack));

        sequence.OnComplete(() => isAnimate = false);

        while (isAnimate)
            await UniTask.Yield();
    }
}