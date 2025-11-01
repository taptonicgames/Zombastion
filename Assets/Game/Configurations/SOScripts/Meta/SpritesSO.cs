using UnityEngine;

[CreateAssetMenu(fileName = "SpritesSO", menuName = "ScriptableObjects/SpritesSO")]
public class SpritesSO : ScriptableObject
{
    //[field: SerializeField] public CurrencyData[] Datas { get; private set; }
    [field: SerializeField] public CurrencySpritesData[] CurrencyDatas { get; private set; }
    [field: SerializeField] public EquipmentSpritesData[] EquipmentDatas { get; private set; }
    [field: SerializeField] public InsertSpritesData[] InsertDatas { get; private set; }
    [field: SerializeField] public int StartMaxEnergy { get; private set; }
    [field: SerializeField] public RarityUIData[] RarityDatas { get; private set; }

    private void OnValidate()
    {
        //foreach (var data in Datas)
        //    if (data.Id != $"{data.Type}")
        //        data.Id = $"{data.Type}";

        foreach (var data in RarityDatas)
            data.Validate();

        foreach(var data in CurrencyDatas)
            data.Validate();

        foreach (var data in EquipmentDatas)
            data.Validate();

        foreach(var data in InsertDatas)
            data.Validate();
    }
}