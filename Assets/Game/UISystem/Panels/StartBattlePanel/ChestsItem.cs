using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestsItem : MonoBehaviour
{
    [SerializeField] private TMP_Text tittle;
    [SerializeField] private Image levelIcon;
    [SerializeField] private Slider progressBar;
    [SerializeField] private ChestButton[] chestButtons;

    private ChestsSavingData chestsSavingData;
    private GeneralSavingData generalSavingData;
    private BattleSavingData battleSavingData;
    private int currentStage;

    public event Action<ChestsItem, int> ChestOpened;

    private void Awake()
    {
        for (int i = 0; i < chestButtons.Length; i++)
            chestButtons[i].ButtonClicked += OnButtonClicked;
    }

    public void Init(int level, SavingManager savingManager, Color color)
    {
        chestsSavingData = savingManager.GetSavingData<ChestsSavingData>(SavingDataType.Chests);
        generalSavingData = savingManager.GetSavingData<GeneralSavingData>(SavingDataType.General);
        battleSavingData = savingManager.GetSavingData<BattleSavingData>(SavingDataType.Battle);

        currentStage = level;
        int chestLevel = chestsSavingData.GetChestLevel(currentStage);

        tittle.SetText($"normal stage {currentStage + 1}");
        levelIcon.color = color;

        for (int i = 0; i < chestButtons.Length; i++)
        {
            if (i < chestLevel)
            {
                chestButtons[i].Open();
                progressBar.value = i * 0.5f;
            }
            else if (HasOpen(i, chestButtons[i]))
            {
                chestButtons[i].ChangeVisualEnableState(true);
            }
        }
    }

    private void OnButtonClicked(ChestButton chestButton)
    {
        for (int i = 0; i < chestButtons.Length; i++)
        {
            if (HasOpen(i, chestButton))
            {
                ChestOpened?.Invoke(this, i);
                chestButtons[i].Open();
                progressBar.value = i * 0.5f;

                EventBus<ChestOpenedEvnt>.Publish(
                    new ChestOpenedEvnt() { roundIndex = currentStage, chestLevel = i + 1 });

                if (i < chestButtons.Length - 1 && HasOpen(i + 1, chestButtons[i + 1]))
                    chestButtons[i + 1].ChangeVisualEnableState(true);
            }
        }
    }

    private bool HasOpen(int index, ChestButton chestButton)
    {
        int chestLevel = chestsSavingData.GetChestLevel(currentStage);
        int completeRoundsAmount = generalSavingData.GetParamById(Constants.ROUNDS_COMPLETED);

        if (chestButtons[index] == chestButton &&
            index == chestLevel &&
            currentStage < completeRoundsAmount)
            return true;

        return false;
    }

    private void OnDestroy()
    {
        for (int i = 0; i < chestButtons.Length; i++)
            chestButtons[i].ButtonClicked -= OnButtonClicked;
    }
}