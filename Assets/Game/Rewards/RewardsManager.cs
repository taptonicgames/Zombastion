using UnityEngine;
using Zenject;

public class RewardsManager : IInitializable
{
    [Inject] private SharedObjects sharedObjects;

    private LevelRewardsSO levelRewardsSO;

    public void Initialize()
    {
        levelRewardsSO = sharedObjects.GetScriptableObject<LevelRewardsSO>(Constants.LEVEL_REWARDS_SO);
    }

    public LevelRewardData GetLevelRewardData(int level)
    {
        if (levelRewardsSO == null)
            levelRewardsSO = sharedObjects.GetScriptableObject<LevelRewardsSO>(Constants.LEVEL_REWARDS_SO);

        return levelRewardsSO.Datas[level];
    }
}