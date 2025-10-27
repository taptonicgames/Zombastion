using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class StartBattlePanel : AbstractPanel
{
    [SerializeField] private Button battleButton;
    [SerializeField] private SceneLoader sceneLoader;

    [SerializeField] private CurrencyType battlePriceType;
    [SerializeField] private Image battlePriceIcon;
    [SerializeField] private TMP_Text battlePriceText;

    [Space(10), Header("Level view")]
    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Image levelIcon;
    [SerializeField] private TMP_Text levelTittle;
    [SerializeField] private Color[] colors;
    [SerializeField] private float animateStepDuration = 0.5f;

    [Inject] private CurrencyManager currencyManager;
    [Inject] private AbstractSavingManager savingsManager;

    private GeneralSavingData generalSavingData;
    private Sequence sequence;
    private int counter;
    private int amountCompleteRounds;

    private const int START_BATTLE_PRICE = 5;

    public override PanelType Type => PanelType.Start;

    public override void Init()
    {
        CurrencyData currencyData = currencyManager.GetCurrencyData(battlePriceType);
        battlePriceIcon.sprite = currencyData.UIData.Icon;
        generalSavingData = savingsManager.GetSavingData<GeneralSavingData>(SavingDataType.General);
        levelIcon.color = colors[generalSavingData.GetParamById(Constants.ROUNDS_COMPLETED)];
        amountCompleteRounds = generalSavingData.GetParamById(Constants.ROUNDS_COMPLETED);
        counter = amountCompleteRounds;

        Subscrube();

        UpdateInfo(false);
    }

    private void UpdateInfo(bool isAnimate = true)
    {
        if (isAnimate)
        {
            sequence.Kill();
            sequence = DOTween.Sequence();
            sequence.Append(levelIcon.transform.DOScale(Vector3.zero, animateStepDuration).From(Vector3.one).SetEase(Ease.Linear));
            sequence.AppendCallback(() => levelIcon.color = colors[counter]);
            sequence.Append(levelIcon.transform.DOScale(Vector3.one, animateStepDuration).SetEase(Ease.OutBack));
        }

        battleButton.interactable = currencyManager.HasPurchased(battlePriceType, START_BATTLE_PRICE);
        prevButton.gameObject.SetActive(counter > 0);
        nextButton.gameObject.SetActive(counter < amountCompleteRounds);

        levelTittle.SetText($"normal stage {counter + 1}");
    }

    #region Events
    private void Subscrube()
    {
        battleButton.onClick.AddListener(OnBattleButtonClicked);
        prevButton.onClick.AddListener(OnPrevButtonClicked);
        nextButton.onClick.AddListener(OnNextButtonClicked);
        currencyManager.CurrencyChanged += OnCurrencyChanged;
    }

    private void Unsubscribe()
    {
        battleButton.onClick.RemoveListener(OnBattleButtonClicked);
        prevButton.onClick.RemoveListener(OnPrevButtonClicked);
        nextButton.onClick.RemoveListener(OnNextButtonClicked);
        currencyManager.CurrencyChanged -= OnCurrencyChanged;
    }

    private void OnBattleButtonClicked()
    {
        int currentCurrency = currencyManager.GetCurrencyAmount(battlePriceType);

        if (currencyManager.HasPurchased(battlePriceType, START_BATTLE_PRICE))
        {
            currencyManager.RemoveCurrency(battlePriceType, START_BATTLE_PRICE);
            generalSavingData.SetParamById(Constants.ROUND_PICKED, counter);
            sceneLoader.LoadScene(1);
        }
    }

    private void OnPrevButtonClicked()
    {
        counter--;
        UpdateInfo();
    }

    private void OnNextButtonClicked()
    {
        counter++;
        UpdateInfo();
    }

    private void OnCurrencyChanged(CurrencyType type)
    {
        if (type == battlePriceType)
            UpdateInfo(false);
    }
    #endregion

    private void OnDestroy()
    {
        Unsubscribe();

        sequence.Kill();
    }
}