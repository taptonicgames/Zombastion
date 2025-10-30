using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonClickInvoker : MonoBehaviour
{
    private Button button;

    public Action<ButtonClickInvoker> ButtonClicked;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClicked);
    }

    public void ChangeButtonInteractableState(bool isEnable)
    {
        button.interactable = isEnable;
    }

    private void OnButtonClicked()
    {
        ButtonClicked.Invoke(this);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnButtonClicked);
    }
}