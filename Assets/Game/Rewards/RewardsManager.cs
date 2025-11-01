using UnityEngine;
using Zenject;

public class RewardsManager : IInitializable
{
    [Inject] private SharedObjects sharedObjects;

    private LevelRewardsSO levelRewardsSO;
    private ChestRewardsSO chestRewardsSO;

    public void Initialize()
    {
        levelRewardsSO = sharedObjects.GetScriptableObject<LevelRewardsSO>(Constants.LEVEL_REWARDS_SO);
        chestRewardsSO = sharedObjects.GetScriptableObject<ChestRewardsSO>(Constants.CHEST_REWARDS_SO);
    }

    public LevelRewardData GetLevelRewardData(int level)
    {
        if (levelRewardsSO == null)
            levelRewardsSO = sharedObjects.GetScriptableObject<LevelRewardsSO>(Constants.LEVEL_REWARDS_SO);

        return levelRewardsSO.Datas[level];
    }

    public ChestRewardData GetChestRewardData(int level)
    {
        if (chestRewardsSO == null)
            chestRewardsSO = sharedObjects.GetScriptableObject<ChestRewardsSO>(Constants.CHEST_REWARDS_SO);

        return chestRewardsSO.Datas[level];
    }

    public RewardData GetRewardDatas(ChestRewardData rewardData, int index)
    {
        return index switch
        {
            0 => rewardData.CommonChestDatas,
            1 => rewardData.RareChestDatas,
            2 => rewardData.LegendChestDatas,
            _ => new RewardData()
        };
    }
}