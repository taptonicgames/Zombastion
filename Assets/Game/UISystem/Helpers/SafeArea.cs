using UnityEngine;

public class SafeArea : MonoBehaviour
{
    private void Start()
    {
        InitSafeArea();
    }

    private void InitSafeArea()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();

        Rect safeArea = Screen.safeArea;
        int screenWidth = Screen.width;
        int screenHeight = Screen.height;

        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;
        anchorMin.x /= screenWidth;
        anchorMin.y /= screenHeight;
        anchorMax.x /= screenWidth;
        anchorMax.y /= screenHeight;

        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
    }
}