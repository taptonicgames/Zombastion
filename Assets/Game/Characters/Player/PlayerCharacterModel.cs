using UniRx;

public class PlayerCharacterModel
{
	//private int experience;

	//public int Experience
	//{
 //       get => experience;
 //       set
 //       {
 //           experience = value;
 //           ExperienceRP.Value = value;
 //       }
 //   }

    public ReactiveProperty<int> Experience { get; set; } = new();

	public void ResetParameters()
	{
		Experience.Value = 0;
	}
}
