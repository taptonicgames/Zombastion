using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestsPopup : AbstractPopup
{
    [SerializeField] private ChestsItem chestsItemPrefab;
    [SerializeField] private Transform itemsContainer;
    [SerializeField] private Transform rewardsView;
    [SerializeField] private Transform closeView;
    [SerializeField] private EndRoundRewardCellsContainer rewardCellsContainer;
    [SerializeField] private Button closeRewardsViewButton;

    private List<ChestsItem> items = new List<ChestsItem>();
    private GeneralSavingData generalSavingData;
    private RewardsManager rewardsManager;
    private CurrencyManager currencyManager;
    private ChestsSavingData chestsSavingData;
    private Tween tween;

    public override void Init(object[] args)
    {
        SavingManager savingManager = (SavingManager)args[0];
        rewardsManager = (RewardsManager)args[1];
        currencyManager = (CurrencyManager)args[2];
        Color[] colors = (Color[])args[3];

        generalSavingData = savingManager.GetSavingData<GeneralSavingData>(SavingDataType.General);
        chestsSavingData = savingManager.GetSavingData<ChestsSavingData>(SavingDataType.Chests);

        closeRewardsViewButton.onClick.AddListener(OnCloseButtonClicked);
        OnCloseButtonClicked();

        int openLevels = generalSavingData.GetParamById(Constants.ROUNDS_COMPLETED);

        for (int i = 0; i <= openLevels; i++)
        {
            ChestsItem item = Instantiate(chestsItemPrefab, itemsContainer);
            item.Init(i, savingManager, colors[i]);
            item.ChestOpened += OnChestOpened;
            items.Add(item);
        }
    }

    private void OnChestOpened(ChestsItem chestsItem, int chestIndex)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (chestsItem == items[i])
            {
                RewardData[] rewardDatas = rewardsManager.GetRewardDatas(rewardsManager.GetChestRewardData(i), chestIndex);
                rewardCellsContainer.Init(rewardDatas);
                ApplyRewards(rewardDatas);

                AnimateChest();
            }
        }
    }

    private async void AnimateChest()
    {
        closeRewardsViewButton.gameObject.SetActive(true);
        rewardsView.gameObject.SetActive(true);
        closeView.gameObject.SetActive(true);

        closeRewardsViewButton.transform.localScale = Vector3.zero;
        rewardsView.transform.localScale = Vector3.zero;
        closeView.transform.localScale = Vector3.zero;

        await AnimateElement(closeRewardsViewButton.transform);
        await AnimateElement(rewardsView.transform);
        await rewardCellsContainer.AnimateCells();
        await AnimateElement(closeView.transform);

    }

    private void ApplyRewards(RewardData[] rewardDatas)
    {
        for (int i = 0; i < rewardDatas.Length; i++)
            currencyManager.AddCurrency(rewardDatas[i].CurrencyType, rewardDatas[i].Value);
    }

    private async UniTask AnimateElement(Transform element)
    {
        bool isAnimate = true;

        tween?.Kill();
        tween = element.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack);
        tween.OnComplete(() => isAnimate = false);

        while (isAnimate)
            await UniTask.Yield();
    }

    private void OnCloseButtonClicked()
    {
        closeRewardsViewButton.gameObject.SetActive(false);
        rewardsView.gameObject.SetActive(false);
        closeView.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        tween?.Kill();

        closeRewardsViewButton.onClick.RemoveListener(OnCloseButtonClicked);

        foreach (var item in items)
            item.ChestOpened -= OnChestOpened;
    }
}