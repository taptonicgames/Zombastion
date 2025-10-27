using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSO", menuName = "ScriptableObjects/PlayerSO")]
public class PlayerSO : ScriptableObject, IGetAttackSOParameters, IScriptableObjectData
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private int shootDamage;

    [SerializeField]
    private float shootDelay;

    [SerializeField]
    private int health;

    [SerializeField]
    private int critDamage;

    [SerializeField]
    private float critProbability;

    [field: SerializeField]
    public int HealthResurectionPerSecond { get; private set; }

    [field: SerializeField]
	public List<AdditionalParameters> additionalParameters { get; private set; }

	public float Speed => speed;
    public int Damage => shootDamage;
    public float ShootDelay => shootDelay;
    public int Health => health;
    public int CritDamage => critDamage;
    public float CritProbability => critProbability;

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
