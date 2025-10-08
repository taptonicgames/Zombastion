using Zenject;

public class MetaInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<MetaSceneManager>().AsSingle();
    }
}