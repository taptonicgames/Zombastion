using System;
using UnityEngine;

[Serializable]
public class UpgradeData
{
    [field: SerializeField] public string Id { get; private set; }
    [field: SerializeField] public PriceData[] Datas { get; private set; }

    public void Validate()
    {
        Id = Datas[Datas.Length - 1].CurrencyType.ToString();
    }
}