using System.Linq;
using UnityEngine;
using Zenject;

public class SpritesManager : IInitializable
{
    [Inject] private SharedObjects sharedObjects;

    private SpritesSO spritesSO;

    public void Initialize()
    {
        spritesSO = sharedObjects.GetScriptableObject<SpritesSO>(Constants.SPRITES_SO);
    }

    public Sprite GetIconSprite(AbstractSpritesData spritesData)
    {
        if (spritesSO == null)
            spritesSO = sharedObjects.GetScriptableObject<SpritesSO>(Constants.SPRITES_SO);

        return spritesData switch
        {
            CurrencySpritesData currency => spritesSO.CurrencyDatas.FirstOrDefault(d => d.Type == currency.Type).Icon,
            EquipmentSpritesData equipment => spritesSO.EquipmentDatas.FirstOrDefault(d => d.Type == equipment.Type).Icon,
            InsertSpritesData insert => spritesSO.InsertDatas.FirstOrDefault(d => d.Type == insert.Type).Icon,
            _ => null
        };
    }

    public Sprite GetBackgroundSprite(RarityType rarityType)
    {
        if (spritesSO == null)
            spritesSO = sharedObjects.GetScriptableObject<SpritesSO>(Constants.SPRITES_SO);

        return spritesSO.RarityDatas.FirstOrDefault(d => d.Type == rarityType).Sprite;
    }

    public void UpdateSprites(AbstractRewardData abstractRewardData, ref Sprite icon, ref Sprite bg)
    {
        switch (abstractRewardData)
        {
            case CurrencyRewardData currencyRewardData:
                icon = GetIconSprite(new CurrencySpritesData(currencyRewardData.Type));
                bg = GetBackgroundSprite(RarityType.Common);
                break;
            case EquipmentRewardData equipmentRewardData:
                icon = GetIconSprite(new EquipmentSpritesData(equipmentRewardData.Type));
                bg = GetBackgroundSprite(equipmentRewardData.Rarity);
                break;
            case InsertRewardData insertRewardData:
                icon = GetIconSprite(new InsertSpritesData(insertRewardData.Type));
                bg = GetBackgroundSprite(insertRewardData.Rarity);
                break;
        }
    }
}