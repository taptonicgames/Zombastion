using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchButtonsController : MonoBehaviour
{
    [SerializeField] private Button firstButton;
    [SerializeField] private Button secondButton;

    [Space(10)]
    [SerializeField] private Transform fillFirstButton;
    [SerializeField] private Transform fillSecondButton;

    [Space(10)]
    [SerializeField] private IndicatorAnimator firstButtonIndicaor;
    [SerializeField] private IndicatorAnimator secondsButtonIndicaor;

    public Button FirstButton => firstButton;
    public Button SecondButton => secondButton;

    private bool firstShowed;
    private bool secondShowed;

    public event Action FirstButtonClicked;
    public event Action SecondButtonClicked;

    public void Init()
    {
        firstButton.onClick.AddListener(OnFirstButtonClicked);
        secondButton.onClick.AddListener(OnSecondButtonClicked);

        firstButtonIndicaor.gameObject.SetActive(false);
        secondsButtonIndicaor.gameObject.SetActive(false);
    }

    public void Show()
    {
        OnFirstButtonClicked();
    }

    public void ChangeInteractableState(bool isInteractable)
    {
        firstButton.interactable = isInteractable;
        secondButton.interactable = isInteractable;
    }

    public void ShowFirstButtonIndicator()
    {
        if (firstShowed)
            return;

        firstShowed = true;
        firstButtonIndicaor.gameObject.SetActive(true);
    }

    public void ShowSecondButtonIndicator()
    {
        if (secondShowed)
            return;

        secondShowed = true;
        secondsButtonIndicaor.gameObject.SetActive(true);
    }

    private void OnFirstButtonClicked()
    {
        firstButtonIndicaor.gameObject.SetActive(false);

        fillSecondButton.gameObject.SetActive(false);
        secondButton.interactable = true;

        fillFirstButton.gameObject.SetActive(true);
        firstButton.interactable = false;

        FirstButtonClicked?.Invoke();
    }

    private void OnSecondButtonClicked()
    {
        secondsButtonIndicaor.gameObject.SetActive(false);

        fillSecondButton.gameObject.SetActive(true);
        secondButton.interactable = false;

        fillFirstButton.gameObject.SetActive(false);
        firstButton.interactable = true;

        SecondButtonClicked?.Invoke();
    }

    private void OnDestroy()
    {
        firstButton.onClick.RemoveListener(OnFirstButtonClicked);
        secondButton.onClick.RemoveListener(OnSecondButtonClicked);
    }
}
