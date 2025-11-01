using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "ScriptableObjects/EnemySO")]
public class EnemySO : ScriptableObject, IGetAttackSOParameters
{
    [field: SerializeField]
    public int Damage { get; private set; }

    [field: SerializeField]
    public float ShootDelay { get; private set; }

    [field: SerializeField]
    public float Speed { get; private set; }

    [field: SerializeField]
    public int Health { get; private set; }

    [field: SerializeField]
    public int ExperienceForDestroy { get; private set; }

    [field: SerializeField]
    public List<AdditionalParameters> additionalParameters { get; private set; }

	private object GetValueByTag(string tag, System.Type returnType)
	{
		var parameters = additionalParameters.Find(a => a.paramName == tag);

		if (parameters.paramName != "")
		{
			if (returnType == typeof(float))
			{
				System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("en-US");
				return float.Parse(parameters.value, cultureInfo);
			}

			if (returnType == typeof(string)) return parameters.value;
		}

		return null;
	}

	public T GetValueByTag<T>(string tag)
	{
		var value = (T)GetValueByTag(tag, typeof(T));
		return value != null ? value : default(T);
	}
}
