using Zenject;

public class MetaInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<EquipmentManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<CurrencyManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<RewardsManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<TowersManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<CardsUpgradeManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<MetaSceneManager>().AsSingle();
    }
}