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
}
