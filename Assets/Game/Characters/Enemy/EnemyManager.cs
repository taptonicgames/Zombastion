using Cysharp.Threading.Tasks;
using System.Collections.Generic;
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

    public void Initialize()
    {
        FindEnemies().Forget();
    }

    private async UniTask FindEnemies()
    {
        CharacterType[] enemyTypes = { CharacterType.SimpleZombie };

        while (enemies.Count < gamePreferences.totalEnemiesAmount)
        {
            var enemyType = enemyTypes[Random.Range(0, enemyTypes.Length)];

            var spawnArea = sceneReferences.enemiesSpawnAreas[
                Random.Range(0, sceneReferences.enemiesSpawnAreas.Count)
            ];

            var enemies = characterFactory.Spawn(enemyType, 1, spawnArea);
            this.enemies.AddRange(enemies);
            await UniTask.WaitForSeconds(gamePreferences.enemySpawnDelay);
        }

        Debug.Log(enemies.Count);
        await UniTask.WaitUntil(() => enemies.Count < gamePreferences.totalEnemiesAmount);
        Debug.Log(enemies.Count);
        FindEnemies().Forget();
    }

    public void RemoveEnemyFromList(AbstractUnit enemy)
    {
        enemies.Remove(enemy);
    }
}
