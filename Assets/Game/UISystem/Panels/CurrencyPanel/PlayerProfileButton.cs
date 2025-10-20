using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProfileButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image proggressFill;
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text levelText;

    public event Action ProfileButtonClicked;

    private void Awake()
    {
        button.onClick.AddListener(OnButtonClicked);
    }

    public void Init()
    {
        //TODO: implement player profile params
        nameText.SetText($"profile name");
        levelText.SetText($"{1}");
        proggressFill.fillAmount = 0.66f;
    }

    private void OnButtonClicked()
    {
        ProfileButtonClicked?.Invoke();
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnButtonClicked);
    }
}