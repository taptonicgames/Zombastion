using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ChestButton : MonoBehaviour
{
    [SerializeField] private ButtonClickInvoker buttonClickInvoker;
    [SerializeField] private Image chestIcon;
    [SerializeField] private Sprite completeSprite;
    [SerializeField] private Image elipse;

    public event Action<ChestButton> ButtonClicked;

    private void Awake()
    {
        buttonClickInvoker.ButtonClicked += OnButtonClicked;
        elipse.gameObject.SetActive(false);
    }

    public void Open()
    {
        buttonClickInvoker.ChangeButtonInteractableState(false);
        ChangeVisualEnableState(false);
        chestIcon.sprite = completeSprite;
    }

    public void ChangeVisualEnableState(bool isEnable)
    {
        elipse.gameObject.SetActive(isEnable);
    }

    private void OnButtonClicked(ButtonClickInvoker invoker)
    {
        ButtonClicked?.Invoke(this);
    }

    private void OnDestroy()
    {
        buttonClickInvoker.ButtonClicked -= OnButtonClicked;
    }
}