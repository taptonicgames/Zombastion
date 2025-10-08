using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItemsBag : MonoBehaviour
{
    [SerializeField] private SwitchButtonsController switchButtonsController;

    [Space(10), Header("Bag Items containers")]
    [SerializeField] private Transform clothItemsBagScrollView;
    [SerializeField] private Transform clothItemsBagContainer;

    [SerializeField] private Transform insertsScrollView;
    [SerializeField] private Transform insertsContainer;

    public void Init()
    {
        switchButtonsController.Init();

        Subscribe();
    }

    public void Show()
    {
        switchButtonsController.Show();
    }

    private void Subscribe()
    {
        switchButtonsController.FirstButtonClicked += OnClothItemsBagButtonClicked;
        switchButtonsController.SecondButtonClicked += OnInsertItemsBagButtonClicked;
    }

    private void OnClothItemsBagButtonClicked()
    {
        insertsScrollView.gameObject.SetActive(false);
        clothItemsBagScrollView.gameObject.SetActive(true);
    }

    private void OnInsertItemsBagButtonClicked()
    {
        clothItemsBagScrollView.gameObject.SetActive(false);
        insertsScrollView.gameObject.SetActive(true);
    }

    private void Unsubscribe()
    {
        switchButtonsController.FirstButtonClicked -= OnClothItemsBagButtonClicked;
        switchButtonsController.SecondButtonClicked -= OnInsertItemsBagButtonClicked;
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }
}