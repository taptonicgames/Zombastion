using Zenject;

public class SceneFunctionalsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
		Container.BindInterfacesAndSelfTo<EnemyManager>().AsSingle();
		Container.Bind<CharacterFactory>().AsTransient();
		Container.Bind<PlayerCharacterModel>().AsSingle();
		Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();
	}
}