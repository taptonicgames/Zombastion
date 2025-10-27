using PlasticGui.WorkspaceWindow.History;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeTowerButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text tittleText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text unlockLevelText;
    [SerializeField] private Image icon;

    [Space(10), Header("States")]
    [SerializeField] private Transform lockStateView;
    [SerializeField] private Transform ButtomView;

    [Space(10), Header("Progress view")]
    [SerializeField] private Slider progressBar;
    [SerializeField] private TMP_Text progressText;
    [SerializeField] private IndicatorAnimator upgradeIndicator;

    private TowersManager towersManager;

    public TowerSO TowerSO { get; private set; }

    public event Action<UpgradeTowerButton> ButtonClicked;

    private void Awake()
    {
        button.onClick.AddListener(OnButtonClicked);
    }

    public void Init(TowerSO towerSO, TowersManager towersManager)
    {
        TowerSO = towerSO;
        this.towersManager = towersManager;

        UpdateInfo();
    }

    public void UpdateInfo()
    {
        tittleText.SetText($"{TowerSO.UIData.Name}");
        levelText.SetText($"{towersManager.GetTowerLevel(TowerSO.Id)}");
        unlockLevelText.SetText($"{1}");
        icon.sprite = TowerSO.UIData.Icon;

        lockStateView.gameObject.SetActive(false);
    }

    private void OnButtonClicked()
    {
        ButtonClicked?.Invoke(this);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnButtonClicked);
    }
}