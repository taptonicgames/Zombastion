using UnityEngine;

public class PercentageCalculator : MonoBehaviour
{
    private static readonly int _attemptsCount = 20;

    public static float ConvertToPercentage(float currentValue, float maxValue)
    {
        return (currentValue / maxValue * 100);
    }

    public static float ConvertToValue(float value, float percent)
    {
        return value - (value * (1 - percent * 0.01f));
    }

    public static Vector3 ConvertToValue(Vector3 value, float percent)
    {
        return value - (value * (1 - percent * 0.01f));
    }

    public static bool CalculateChanceResponce(float responsePercentage)
    {
        float random = Random.Range(0, 1f);
        float total = 100;

        if (responsePercentage / total >= random) return true;
        else return false;
    }

    public static int CalculateVariant(float[] variants)
    {
        int result = -1;

        for (int i = 0; i < _attemptsCount; i++)
        {
            result = Calculate(variants);
            if (result != -1)
                return result;
        }

        return 0;
    }

    private static int Calculate(float[] variants)
    {
        float random = Random.Range(0, 1f);
        float numForAdding = 0;
        float total = 100;

        for (int i = 0; i < variants.Length; i++)
        {
            total += variants[i];
        }

        for (int i = 0; i < variants.Length; i++)
        {
            if (variants[i] / total + numForAdding >= random)
                return i;
            else
                numForAdding += variants[i] / total;
        }

        return -1;
    }
}