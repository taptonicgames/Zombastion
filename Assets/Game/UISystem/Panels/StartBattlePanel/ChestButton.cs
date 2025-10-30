using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ChestButton : MonoBehaviour
{
    [SerializeField] private ButtonClickInvoker buttonClickInvoker;
    [SerializeField] private Image chestIcon;
    [SerializeField] private Sprite completeSprite;

    public event Action<ChestButton> ButtonClicked;

    private void Awake()
    {
        buttonClickInvoker.ButtonClicked += OnButtonClicked;
    }

    public void Open()
    {
        buttonClickInvoker.ChangeButtonInteractableState(false);
        chestIcon.sprite = completeSprite;
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