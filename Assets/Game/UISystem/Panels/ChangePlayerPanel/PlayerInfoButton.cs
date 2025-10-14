using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static PlayerCosmeticSO;

public class PlayerInfoButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image background;
    [SerializeField] private Image icon;
    [SerializeField] private Transform openCharacterObject;
    [SerializeField] private Transform comingSoonObject;
    [SerializeField] private Sprite pickedSprite;
    [SerializeField] private Sprite notPickedSprite;

    public PlayerCosmeticData Data { get; private set; }

    public event Action<PlayerInfoButton> ButtonClicked;

    private void Awake()
    {
        button.onClick.AddListener(OnButtonClicked);
    }

    public void Init(PlayerCosmeticData data)
    {
        Data = data;
        icon.sprite = data.PlayerIcon;
    }

    public void SetComingSoonState()
    {
        openCharacterObject.gameObject.SetActive(false);
        comingSoonObject.gameObject.SetActive(true);
        button.interactable = false;
    }

    public void ChangePickedState(bool isPicked)
    {
        background.sprite = isPicked ? pickedSprite : notPickedSprite;
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