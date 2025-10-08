using UnityEngine;
using UnityEngine.UI;

public class InsertItemView : MonoBehaviour
{
    [SerializeField] private Image activeIcon;
    [SerializeField] private RectTransform view;
    [SerializeField] private Vector2 portretOrientationSize;
    [SerializeField] private Vector2 albomOrientationSize;

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