using UnityEngine;

public abstract class BasePanel : MonoBehaviour
{
    [field: SerializeField] public Transform Panel { get; private set; }

    public abstract PanelType PanelType { get; }

    public virtual void Show()
    {
        Panel.gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        Panel.gameObject.SetActive(false);
    }
}