using UnityEngine;
using UnityEngine.UI;

public class ClothItemsContainer : MonoBehaviour
{
    [SerializeField] private Vector2 portretOrientationResolution;
    [SerializeField] private Vector2 albomOrientationResolution;

    private ClothItem[] items;
    private GridLayoutGroup gridLayoutGroup;

    public void Init()
    {
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
        items = GetComponentsInChildren<ClothItem>();

        gridLayoutGroup.cellSize = ScreenExtension.IsPortretOrientation ? portretOrientationResolution : albomOrientationResolution;

        for (int i = 0; i < items.Length; i++)
        {
            items[i].Init();
            items[i].ItemClicked += OnItemClicked;
        }
    }

    private void OnItemClicked(ClothItem item)
    {
        //TODO: open item popup
    }

    private void OnDestroy()
    {
        for (int i = 0; i < items.Length; i++)
            items[i].ItemClicked -= OnItemClicked;
    }

    #region Debug
#if UNITY_EDITOR
    private bool isPortretOrientation;

    private void Update()
    {
        if (isPortretOrientation != ScreenExtension.IsPortretOrientation)
        {
            isPortretOrientation = ScreenExtension.IsPortretOrientation;
            gridLayoutGroup.cellSize = ScreenExtension.IsPortretOrientation ? portretOrientationResolution : albomOrientationResolution;
        }
    }
#endif
    #endregion
}