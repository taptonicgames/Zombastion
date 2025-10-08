using UnityEngine;
using UnityEngine.UI;

public static class ScrollViewScroller
{
    public static void VerticalScrollToTarget(this ScrollRect scrollRect, RectTransform contentContainer, RectTransform targetObj)
    {
        scrollRect.normalizedPosition = scrollRect.CalculateFocusedScrollPosition(targetObj);
    }

    private static Vector2 CalculateFocusedScrollPosition(this ScrollRect scrollRect, RectTransform targetObj)
    {
        Vector2 itemCenterPoint = scrollRect.content.InverseTransformPoint(targetObj.transform.TransformPoint(targetObj.rect.center));
        Vector2 contentSizeOffset = scrollRect.content.rect.size;
        contentSizeOffset.Scale(scrollRect.content.pivot);

        return scrollRect.CalculateFocusedScrollPosition(itemCenterPoint + contentSizeOffset);
    }

    private static Vector2 CalculateFocusedScrollPosition(this ScrollRect scrollView, Vector2 focusPoint)
    {
        Vector2 contentSize = scrollView.content.rect.size;
        Vector2 viewportSize = ((RectTransform)scrollView.content.parent).rect.size;
        Vector2 contentScale = scrollView.content.localScale;

        contentSize.Scale(contentScale);
        focusPoint.Scale(contentScale);

        Vector2 scrollPosition = scrollView.normalizedPosition;
        if (scrollView.horizontal && contentSize.x > viewportSize.x)
            scrollPosition.x = Mathf.Clamp01((focusPoint.x - viewportSize.x * 0.5f) / (contentSize.x - viewportSize.x));
        if (scrollView.vertical && contentSize.y > viewportSize.y)
            scrollPosition.y = Mathf.Clamp01((focusPoint.y - viewportSize.y * 0.5f) / (contentSize.y - viewportSize.y));

        return scrollPosition;
    }
}