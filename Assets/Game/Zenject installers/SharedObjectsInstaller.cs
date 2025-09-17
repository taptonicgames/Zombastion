using System.Collections.Generic;
using Zenject;

public class SharedObjectsInstaller : MonoInstaller
{
	public List<IDGameObjectData> idGameObjectDatasList;

	public override void InstallBindings()
	{
		Container.Bind<SharedObjects>().AsTransient().OnInstantiated<SharedObjects>(OnInstantiated);
	}

	private void OnInstantiated(InjectContext context, SharedObjects objects)
	{
		objects.SetTypeGameObjectDatasList(idGameObjectDatasList);
	}
}