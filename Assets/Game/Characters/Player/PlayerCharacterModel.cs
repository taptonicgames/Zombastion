using UniRx;

public class PlayerCharacterModel
{
	public ReactiveProperty<int> Experience { get; set; } = new();

	public void ResetParameters()
	{
		Experience.Value = 0;
	}
}
