using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CoroutineManagerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container
            .Bind<CoroutineManager>()
            .FromNewComponentOnNewGameObject()
            .AsSingle()
            .OnInstantiated<CoroutineManager>((a, b) => b.gameObject.name = "CoroutineManager");
    }
}

public class CoroutineManager : MonoBehaviour
{
    public Coroutine InvokeActionDelay(Action action, float delay)
    {
        var coroutine = StartCoroutine(ActionCoroutine(action, delay));
        return coroutine;
    }

    public Coroutine InvokeWaitUntil(Action action, Func<bool> func)
    {
        var coroutine = StartCoroutine(ActionCoroutine(action, func));
        return coroutine;
    }

    public Coroutine InvokeDelayedActions(
        List<(Action action, float delayBetweenActions)> actions,
        float delay = 0.1f
    )
    {
        var coroutine = StartCoroutine(ActionCoroutine(actions, delay));
        return coroutine;
    }

    public IEnumerator ActionCoroutine(Action action, float delay = 0.1f)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }

    public IEnumerator ActionCoroutine(Action action, Func<bool> func)
    {
        yield return new WaitUntil(func);
        action?.Invoke();
    }

    public IEnumerator ActionCoroutine(
        List<(Action action, float delayBetweenActions)> actions,
        float delay = 0.1f
    )
    {
        yield return new WaitForSeconds(delay);

        foreach (var item in actions)
        {
            item.action.Invoke();
            yield return new WaitForSeconds(item.delayBetweenActions);
        }
    }
}
