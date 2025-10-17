using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public abstract class AbstractPanel : MonoBehaviour
{
    [SerializeField] protected Transform Panel;

    public abstract PanelType Type { get; }

    public bool IsShown { get; protected set; } = true;

	public virtual void Init() { }
    public virtual void Init(object[] arr) { }

    public async void Show(Action callback = null)
    {
        if (!IsShown)
        {
            IsShown = true;

            Panel.gameObject.SetActive(true);

            await OnShow();

            callback?.Invoke();
        }
    }

    public async void Hide()
    {
        if (IsShown)
        {
            IsShown = false;

            await OnHide();

            Panel.gameObject.SetActive(false);
        }
    }

    public virtual async UniTask OnShow() { }
    public virtual async UniTask OnHide() { }
}