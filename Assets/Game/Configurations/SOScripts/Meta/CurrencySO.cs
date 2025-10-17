using UnityEngine;

[CreateAssetMenu(fileName = "CurrencySO", menuName = "ScriptableObjects/CurrencySO")]
public class CurrencySO : ScriptableObject
{
    [field: SerializeField] public CurrencyData[] Datas { get; private set; }

    private void OnValidate()
    {
        foreach (var data in Datas)
            if (data.Id != $"{data.Type}")
                data.Id = $"{data.Type}";
    }
}