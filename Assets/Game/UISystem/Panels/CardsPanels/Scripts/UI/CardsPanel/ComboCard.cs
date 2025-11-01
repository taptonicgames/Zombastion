using Coffee.UIExtensions;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComboCard : MonoBehaviour
{
    [SerializeField] private Transform elementsContainer;
    [SerializeField] private Image icon;
    [SerializeField] private Image[] levelIcons;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private UIParticle particle;
    [SerializeField] private Sprite completeLevelSprite;

    [Space(10), Header("Animate settings")]
    [SerializeField] private Vector3 targetScale;
    [SerializeField] private float animateStepDuration;

    private Sprite defaultLevelSprite;
    private int completeIconsAmount;
    private int level = 1;

    private Sequence sequence;

    public string Id { get; private set; }
    public bool IsEmpty { get; private set; } = true;

    private void Awake()
    {
        defaultLevelSprite = levelIcons[0].sprite;
        elementsContainer.gameObject.SetActive(false);
    }

    public void Init(BattleUpgradeConfig config)
    {
        Id = config.Id;
        IsEmpty = false;
        elementsContainer.gameObject.SetActive(true);

        levelText.SetText($"{level}");
        icon.sprite = config.UpgradeIcon;
    }

    public async UniTask LevelUp()
    {
        bool isAnimate = true;

        AnimateLevelUp(() =>
        {
            completeIconsAmount++;

            if (completeIconsAmount >= levelIcons.Length)
                TierUp();

            particle.gameObject.SetActive(false);
            isAnimate = false;
        });

        while (isAnimate)
            await UniTask.Yield();

    }

    private void AnimateLevelUp(Action action)
    {
        sequence?.Kill();
        sequence = DOTween.Sequence();
        sequence.Append(levelIcons[completeIconsAmount].transform.DOScale(targetScale, animateStepDuration).SetEase(Ease.Linear));
        sequence.AppendCallback(() =>
        {
            particle.gameObject.SetActive(true);
            particle.transform.position = levelIcons[completeIconsAmount].transform.position;
            levelIcons[completeIconsAmount].sprite = completeLevelSprite;
        });
        sequence.Append(levelIcons[completeIconsAmount].transform.DOScale(Vector3.one, animateStepDuration).SetEase(Ease.Linear));
        sequence.OnComplete(() => action?.Invoke());
    }

    private void TierUp()
    {
        level++;
        completeIconsAmount = 0;

        for (int i = 0; i < levelIcons.Length; i++)
            levelIcons[i].sprite = defaultLevelSprite;

        levelText.SetText($"{level}");
    }
}