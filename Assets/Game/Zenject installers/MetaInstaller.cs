using Zenject;

public class MetaInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<EquipmentManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<MetaSceneManager>().AsSingle();
    }
}