using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class EnemyManager : IInitializable
{
    [Inject]
    private readonly SceneReferences sceneReferences;

    [Inject]
    private readonly CharacterFactory characterFactory;

    [Inject]
    private readonly GamePreferences gamePreferences;
    private List<AbstractUnit> enemies = new();
    private bool pause;

    public void Initialize()
    {
        FindEnemies().Forget();
        EventBus<SetGamePauseEvnt>.Subscribe(OnSetGamePauseEvnt);
    }

	private void OnSetGamePauseEvnt(SetGamePauseEvnt evnt)
	{
        pause = evnt.paused;
	}

	private async UniTask FindEnemies()
    {
        CharacterType[] enemyTypes = { CharacterType.SimpleZombie, CharacterType.ArcherZombie };

        while (enemies.Count < gamePreferences.totalEnemiesAmount)
        {
            var enemyType = enemyTypes[Random.Range(0, enemyTypes.Length)];

            var spawnArea = sceneReferences.enemiesSpawnAreas[
                Random.Range(0, sceneReferences.enemiesSpawnAreas.Count)
            ];

            var enemies = characterFactory.Spawn(enemyType, 1, spawnArea);
            this.enemies.AddRange(enemies);
            await UniTask.WaitForSeconds(gamePreferences.enemySpawnDelay);
			await UniTask.WaitUntil(() => !pause);
		}

        await UniTask.WaitUntil(() => enemies.Count < gamePreferences.totalEnemiesAmount);
        await UniTask.WaitUntil(() => !pause);
        FindEnemies().Forget();
    }

    public void RemoveEnemyFromList(AbstractUnit enemy)
    {
        enemies.Remove(enemy);
    }
}
