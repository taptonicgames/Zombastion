using Zenject;

public class GlobalFunctionalsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<Functionals>().AsTransient().OnInstantiated<Functionals>(OnInstantiated);
    }

    private void OnInstantiated(InjectContext context, Functionals functionals) { }
}
