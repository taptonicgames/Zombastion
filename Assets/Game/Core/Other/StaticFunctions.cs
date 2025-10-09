using System;
using System.Threading;
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
        bool rotate = true
    )
    {
        var targetLocalPos = transform.InverseTransformPoint(targetPos);
        var A = targetLocalPos.x;
        var B = targetLocalPos.z;
        var alpha = Mathf.Atan2(A, B) * Mathf.Rad2Deg;
        var angle = alpha;
        alpha = Mathf.Clamp(alpha, clampMin, clampMax);
        if (rotate)
            transform.Rotate(0, alpha, 0);
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

    public static async void InvokeActionDelay(
        Action action,
        float delay,
        CancellationToken cancellationToken
    )
    {
        await UniTask.WaitForSeconds(delay, cancellationToken: cancellationToken);
        action?.Invoke();
    }
}
