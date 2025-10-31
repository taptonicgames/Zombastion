using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public static class StaticFunctions
{
    public static float CalculateAngle(Transform originalTr, Vector3 targetPos)
    {
        var targetLocalPos = originalTr.InverseTransformPoint(targetPos);
        var A = targetLocalPos.x;
        var B = targetLocalPos.z;
        var alpha = Mathf.Atan2(A, B) * Mathf.Rad2Deg;
        var angle = alpha;
        return angle;
    }

    public static float ObjectFinishTurning(
        Transform transform,
        Vector3 targetPos,
        float clampMin = -10,
        float clampMax = 10,
        bool rotate = true,
        RectTransform.Axis axis = RectTransform.Axis.Horizontal
    )
    {
        var targetLocalPos = transform.InverseTransformPoint(targetPos);
        var A = targetLocalPos.x;
        var B = targetLocalPos.z;

        if (axis == RectTransform.Axis.Vertical)
            A = targetLocalPos.y;

        var alpha = Mathf.Atan2(A, B) * Mathf.Rad2Deg;
        var angle = alpha;
        alpha = Mathf.Clamp(alpha, clampMin, clampMax);

        if (rotate)
        {
            switch (axis)
            {
                case RectTransform.Axis.Horizontal:
                    transform.Rotate(0, alpha, 0);
                    break;
                case RectTransform.Axis.Vertical:
                    transform.Rotate(-alpha, 0, 0, Space.Self);
                    break;
                default:
                    break;
            }
        }

        return angle;
    }

    public static void ObjectFinishTurning(Transform transform, Vector3 targetPos, float duration)
    {
        var targetLocalPos = transform.InverseTransformPoint(targetPos);
        var A = targetLocalPos.x;
        var B = targetLocalPos.z;
        var alpha = Mathf.Atan2(A, B) * Mathf.Rad2Deg;
        var angle = new Vector3(0, alpha, 0);
        transform.DORotate(transform.eulerAngles + angle, duration);
    }

    public static IEnumerable<T> OverlapSphere<T>(Vector3 pos, float radius, bool sort = false)
        where T : Component
    {
        IEnumerable<T> arr;

        arr = Physics
            .OverlapSphere(pos, radius)
            .Where(a => a.GetComponent<T>())
            .Select(a => a.GetComponent<T>());

        if (sort)
        {
            arr = arr.OrderBy(a => Vector3.Distance(a.transform.position, pos));
        }

        return arr;
    }
}
