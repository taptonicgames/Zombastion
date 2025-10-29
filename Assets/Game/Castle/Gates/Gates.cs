using System;
using UnityEngine;
using Zenject;

public class Gates : MonoBehaviour, IDamageReciever
{
    [Inject]
    private readonly SceneReferences sceneReferences;

    [Inject]
    private readonly CoroutineManager coroutineManager;

    [SerializeField]
    private Animator animator;

    public Type GetDamageRecieverType()
    {
        return typeof(Gates);
    }

    public void SetDamage(int damage)
    {
        sceneReferences.castle.SetDamage(damage);
        animator.SetFloat(Constants.DAMAGE, damage);
        coroutineManager.InvokeActionDelay(() => animator.SetFloat(Constants.DAMAGE, 0), 0.2f);
    }

    public void OpenClose(string animName, bool open)
    {
        animator.SetBool(animName, open);
    }
}
