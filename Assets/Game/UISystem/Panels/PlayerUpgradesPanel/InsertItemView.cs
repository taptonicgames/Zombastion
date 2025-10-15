using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class InsertItemView : MonoBehaviour
{
    [SerializeField] private Image activeIcon;
    [SerializeField] private RectTransform view;
    [SerializeField] private Vector2 portretOrientationSize;
    [SerializeField] private Vector2 albomOrientationSize;

    public InsertData InsertData { get; private set; }
    public bool IsActive { get; private set; }

    private void Awake()
    {
        view.sizeDelta = ScreenExtension.IsPortretOrientation ? portretOrientationSize : albomOrientationSize;
    }

    public void Activate()
    {
        IsActive = true;
        activeIcon.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        IsActive = false;
        activeIcon.gameObject.SetActive(false);
    }

    public void SetInsertData(InsertData insertData)
    {
        InsertData = insertData;
        activeIcon.sprite = InsertData.RarityUIData.Icon;
        activeIcon.transform.DOScale(Vector3.one, 0.25f).From(Vector3.zero).SetEase(Ease.OutBack);
        Activate();
    }

    public void ClearData()
    {
        InsertData = null;
        Deactivate();
    }

    #region Debug
#if UNITY_EDITOR
    private bool isPortretOrientation;

    private void Update()
    {
        if (isPortretOrientation != ScreenExtension.IsPortretOrientation)
        {
            isPortretOrientation = ScreenExtension.IsPortretOrientation;
            view.sizeDelta = ScreenExtension.IsPortretOrientation ? portretOrientationSize : albomOrientationSize;
        }
    }
#endif
    #endregion
}