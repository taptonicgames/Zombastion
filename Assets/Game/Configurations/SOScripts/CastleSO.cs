using UnityEngine;

[CreateAssetMenu(fileName = "CastleSO", menuName = "ScriptableObjects/CastleSO")]
public class CastleSO : ScriptableObject
{
	[field: SerializeField] public int Health {  get; private set; }
}