using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class WinRoundPanel : AbstractPanel
{
    [Space(10)]
    [SerializeField] private EndRoundRewardCellsContainer rewardCellsContainer;
    [SerializeField] private EndRoundStatItemsContainer statItemsContainer;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button doubleRewardButton;

    [Space(10), Header("Animate options")]
    [SerializeField] private Transform tittleView;
    [SerializeField] private Transform levelView;
    [SerializeField] private Transform rewardsView;
    [SerializeField] private Transform statItemsView;
    [SerializeField] private float animateDurationPerElement = 0.25f;

    [Inject] private RewardsManager rewardsManager;
    [Inject] private AbstractSavingManager savingManager;
    [Inject] private CurrencyManager currencyManager;

    public override PanelType Type => PanelType.WinRound;

    private void Awake()
    {
        continueButton.onClick.AddListener(OnContinueButtonClicked);
        doubleRewardButton.onClick.AddListener(OnDoubleRewardButtonClicked);
    }

    public override void Init(object[] arr)
    {
        base.Init(arr);

        int level = savingManager.GetSavingData<GeneralSavingData>(SavingDataType.General).GetParamById(Constants.ROUND_PICKED);
        rewardCellsContainer.Init(rewardsManager.GetLevelRewardData(level).WinData);
        statItemsContainer.Init();
    }

    public async override UniTask OnShow()
    {
        await base.OnShow();

        tittleView.localScale = Vector3.zero;
        levelView.transform.localScale = Vector3.zero;
        rewardsView.transform.localScale = Vector3.zero;
        statItemsView.transform.localScale = Vector3.zero;
        continueButton.transform.localScale = Vector3.zero;
        doubleRewardButton.transform.localScale = Vector3.zero;

        await AnimateElement(tittleView.transform);
        await AnimateElement(levelView.transform);
        await AnimateElement(rewardsView.transform);
        await rewardCellsContainer.AnimateCells();
        await AnimateElement(statItemsView.transform);
        await statItemsContainer.AnimateStatItems();
        await AnimateElement(continueButton.transform);
        await AnimateElement(doubleRewardButton.transform);
    }

    private async UniTask AnimateElement(Transform transform)
    {
        bool isAnimate = true;
        transform.DOScale(Vector3.one, animateDurationPerElement).SetEase(Ease.OutBack).OnComplete(() => isAnimate = false);
        while (isAnimate)
            await UniTask.Yield();
    }

    private void OnContinueButtonClicked()
    {
        SceneManager.LoadSceneAsync(0);
    }

    private void OnDoubleRewardButtonClicked()
    {
        currencyManager.SetIncreaseReward(true);
        doubleRewardButton.interactable = false;
        rewardCellsContainer.IncreaseReward(Constants.REWARD_APPLY_AMOUNT);
    }

    private void OnDestroy()
    {
        continueButton.onClick.RemoveListener(OnContinueButtonClicked);
        doubleRewardButton.onClick.RemoveListener(OnDoubleRewardButtonClicked);
    }
}