using Zenject;

public class SavingManagerInstaller : MonoInstaller
{
    public bool dontSave;

    public override void InstallBindings()
    {
        Container
            .Bind(typeof(AbstractSavingManager), typeof(IInitializable))
            .To<SavingManager>()
            .AsSingle()
            .WithArguments(dontSave);
    }
}
