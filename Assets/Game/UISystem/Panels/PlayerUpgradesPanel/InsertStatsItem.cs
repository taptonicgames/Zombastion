using System.Collections;
using UnityEngine;

public class InsertStatsItem : MonoBehaviour
{
    [Header("States")]
    [SerializeField] private RectTransform activateState;
    [SerializeField] private RectTransform deactivateState;

    public void ChangeActiveState(bool isActive)
    {
        activateState.gameObject.SetActive(isActive);
        deactivateState.gameObject.SetActive(!isActive);
    }
}