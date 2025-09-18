using Zenject;

public class FunctionalsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<CharacterFactory>().AsTransient();
        Container.Bind<Functionals>().AsTransient().OnInstantiated<Functionals>(OnInstantiated);
        Container.BindInterfacesAndSelfTo<EnemyManager>().AsSingle();
    }

    private void OnInstantiated(InjectContext context, Functionals functionals) { }
}
