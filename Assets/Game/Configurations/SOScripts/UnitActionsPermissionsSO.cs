using UnityEngine;
using Zenject;

[CreateAssetMenu(
    fileName = "UnitActionsPermissionsSO",
    menuName = "Installers/UnitActionsPermissionsSO"
)]
public class UnitActionsPermissionsSO : ScriptableObjectInstaller<UnitActionsPermissionsSO>
{
    public UnitActionPermissionHandler unitActionPermissionHandler;

    public override void InstallBindings()
    {
        Container
            .BindInterfacesAndSelfTo<UnitActionPermissionHandler>()
            .FromInstance(unitActionPermissionHandler)
            .AsSingle();
    }
}
