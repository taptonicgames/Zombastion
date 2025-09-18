using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "SharedObjectsSO", menuName = "Installers/SharedObjectsSO")]
public class SharedObjectsSO : ScriptableObjectInstaller<SharedObjectsSO>
{
    public SharedObjects sharedObjects;

    public override void InstallBindings()
    {
        Container.BindInstance(sharedObjects).AsTransient();
    }
}
