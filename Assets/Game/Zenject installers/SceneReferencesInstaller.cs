using UnityEngine;
using Zenject;

public class SceneReferencesInstaller : MonoInstaller
{
	public Collider charactersSpawnArea;
	public Transform charactersFolder;
	

	public override void InstallBindings()
	{
		Container.Bind<SceneReferences>().AsTransient().OnInstantiated<SceneReferences>(OnInstantiated);
	}

	private void OnInstantiated(InjectContext context, SceneReferences references)
	{
		references.charactersFolder = charactersFolder;
		references.charactersSpawnArea = charactersSpawnArea;
	}
}