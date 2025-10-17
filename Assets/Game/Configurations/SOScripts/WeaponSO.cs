using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "ScriptableObjects/WeaponSO")]
public class WeaponSO : ScriptableObject
{
    [field: SerializeField]
    public WeaponType WeaponType { get; private set; }

    [SerializeField]
    private float shootDelay;

    [SerializeField]
    private int shootDamage;

    [SerializeField]
    private BulletType bulletType;

    [SerializeField]
    private float bulletSpeed;

    [SerializeField]
    private float attackDistance;

	[field: SerializeField]
	public List<AdditionalParameters> additionalParameters { get; private set; }

	public float ShootDelay => shootDelay;
    public int ShootDamage => shootDamage;
    public BulletType BulletType => bulletType;
    public float BulletSpeed => bulletSpeed;
    public float AttackDistance => attackDistance;

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
