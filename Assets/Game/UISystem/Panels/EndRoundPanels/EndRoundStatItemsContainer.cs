using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;

public class EndRoundStatItemsContainer : MonoBehaviour
{
    [SerializeField] private EndRoundStatItemView[] itemViews;
    [SerializeField] private float animateDurationPerItem = 0.1f;

    private Sequence sequence;

    public void Init()
    {
        for (int i = 0; i < itemViews.Length; i++)
        {
            // TODO: implement result params in the round
            //itemViews[i].Init();
            itemViews[i].transform.localScale = Vector3.zero;
        }
    }

    public async Task AnimateStatItems()
    {
        bool isAnimate = true;

        sequence.Kill();
        sequence = DOTween.Sequence();

        for (int i = 0; i < itemViews.Length; i++)
            sequence.Append(itemViews[i].transform.DOScale(Vector3.one, animateDurationPerItem).SetEase(Ease.OutBack));

        sequence.OnComplete(() => isAnimate = false);

        while(isAnimate)
            await UniTask.Yield();
    }
}