using UnityEngine;

public static class ScreenExtension
{
    public static bool IsPortretOrientation
    {
        get => Screen.width / Screen.height < 1;
    }
}