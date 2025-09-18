using Zenject;

public class SceneReferencesInstaller : MonoInstaller
{
	public SceneReferences sceneReferences;
	

	public override void InstallBindings()
	{
		Container.BindInstance(sceneReferences).AsTransient();
	}
}