using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Obj = UnityEngine.Object;
using Rnd = UnityEngine.Random;

public class ObjectParabolaJumpHelper
{
    public void JumpObjects(JumpObjectData data)
    {
        JumpObjectsAsync(data).Forget();
    }

    private async UniTaskVoid JumpObjectsAsync(JumpObjectData data)
    {
        if (data.randomRotation)
            RotateRandom(data);
        var startPos = data.startPos;
        var index = 0;

        foreach (var item in data.list)
        {
            item.position = startPos;
            var targetPos = data.endTr ? data.endTr.position : data.endPos;
            targetPos.y += data.heightIncrement;
            Vector3 centerPoint = startPos + (targetPos - startPos) / 2;
            centerPoint.y = targetPos.y + 2;

            if (centerPoint.y < startPos.y)
                centerPoint.y = startPos.y + 1;

            var bezierPointA = startPos;
            bezierPointA.y = startPos.y + 1;
            var bezierPointB = startPos + (centerPoint - startPos) / 2;
            bezierPointB.y = centerPoint.y;
            var bezierPointC = centerPoint + (targetPos - centerPoint) / 2;
            bezierPointC.y = centerPoint.y;
            var bezierPointD = targetPos;

            Vector3[] waypoints = new Vector3[]
            {
                centerPoint,
                bezierPointA,
                bezierPointB,
                targetPos,
                bezierPointC,
                bezierPointD,
            };

            index++;
            int remain = data.list.Count - index;

            item.DOPath(
                    waypoints,
                    data.duration,
                    PathType.CubicBezier,
                    PathMode.Full3D,
                    10,
                    Color.red
                )
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    if (data.hideOnFinish)
                        item.gameObject.SetActive(false);
                    if (remain == 0 && data.destroyOnFinish)
                        DestroyItems(data.list);
                    data.CompleteAction?.Invoke(remain);
                });

            await UniTask.Delay(Rnd.Range(10, 70));
        }
    }

    private void DestroyItems(List<Transform> list)
    {
        foreach (Transform item in list)
            Obj.Destroy(item.gameObject);
    }

    private void RotateRandom(JumpObjectData data)
    {
        foreach (var item in data.list)
        {
            item.eulerAngles = new Vector3(Rnd.Range(0, 360), Rnd.Range(0, 360), Rnd.Range(0, 360));
        }
    }

    public struct JumpObjectData
    {
        public List<Transform> list;
        public Vector3 startPos,
            endPos;
        public Transform endTr;
        public bool randomRotation,
            destroyOnFinish,
            hideOnFinish;
        public float heightIncrement,
            duration;
        public Action<int> CompleteAction;
    }
}
