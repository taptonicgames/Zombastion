using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Rnd = UnityEngine.Random;

public class EnemyManager : IInitializable
{
    [Inject]
    private readonly SceneReferences sceneReferences;

    [Inject]
    private readonly CharacterFactory characterFactory;

    [Inject]
    private readonly GamePreferences gamePreferences;
    private List<AbstractUnit> enemiesList = new();
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
        CharacterType[] enemyTypes =
        {
            CharacterType.SimpleZombie,
            CharacterType.ZombieFat, /*, CharacterType.ArcherZombie*/
        };

        while (enemiesList.Count < gamePreferences.totalEnemiesAmount)
        {
            var enemyType = enemyTypes[Rnd.Range(0, enemyTypes.Length)];

            var spawnArea = sceneReferences.enemiesSpawnAreas[
                Rnd.Range(0, sceneReferences.enemiesSpawnAreas.Count)
            ];

            var enemies = characterFactory.Spawn(enemyType, 1, spawnArea).ToArray();
            enemiesList.AddRange(enemies);

            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].name += $"_{enemies[i].GetID()}";
            }

            await UniTask.WaitForSeconds(gamePreferences.enemySpawnDelay);
            await UniTask.WaitUntil(() => !pause);
        }

        await UniTask.WaitUntil(() => enemiesList.Count < gamePreferences.totalEnemiesAmount);
        await UniTask.WaitUntil(() => !pause);
        FindEnemies().Forget();
    }

    public void RemoveEnemyFromList(AbstractUnit enemy)
    {
        enemiesList.Remove(enemy);
    }
}
