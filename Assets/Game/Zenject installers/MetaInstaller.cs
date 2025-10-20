using Zenject;

public class MetaInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<EquipmentManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<CurrencyManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<RewardsManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<MetaSceneManager>().AsSingle();
    }
}