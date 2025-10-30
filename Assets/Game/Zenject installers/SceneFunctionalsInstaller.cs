using UnityEngine;
using Zenject;

public class SceneFunctionalsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<EnemyManager>().AsSingle();
        Container.Bind<CharacterFactory>().AsTransient();
        Container.Bind<PlayerCharacterModel>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<ObjectPoolSystem>().AsSingle();
        Container.Bind<CardsUpgradeManager>().AsSingle();
        Container.Bind<CurrencyManager>().AsSingle();
        Container.Bind<RewardsManager>().AsSingle();
    }
}
