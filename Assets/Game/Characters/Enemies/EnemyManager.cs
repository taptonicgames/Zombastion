using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Rnd = UnityEngine.Random;

public class EnemyManager : IInitializable, IFixedTickable
{
    [Inject]
    private readonly SceneReferences sceneReferences;

    [Inject]
    private readonly CharacterFactory characterFactory;

    [Inject]
    private readonly GamePreferences gamePreferences;
    private List<AbstractUnit> enemiesList = new();
    private bool pause,
        stopCreateEnemy;
    private Timer timer = new Timer(TimerMode.counterFixedUpdate);

    public void Initialize()
    {
        CreateEnemies().Forget();
        EventBus<SetGamePauseEvnt>.Subscribe(OnSetGamePauseEvnt);
        EventBus<RoundCompleteEvnt>.Subscribe(OnRoundCompleteEvnt);
        timer.OnTimerReached += () => stopCreateEnemy = true;
        timer.StartTimer(gamePreferences.roundDuration);
    }

    private void OnRoundCompleteEvnt(RoundCompleteEvnt evnt)
    {
        timer.StopTimer();
        pause = true;
    }

    public void FixedTick()
    {
        timer.TimerUpdate();
    }

    private void OnSetGamePauseEvnt(SetGamePauseEvnt evnt)
    {
        pause = evnt.paused;
    }

    private async UniTask CreateEnemies()
    {
        CharacterType[] enemyTypes =
        {
            CharacterType.SimpleZombie,
            CharacterType.ZombieFat,
            CharacterType.ArcherZombie,
            CharacterType.ZombieAxe,
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
            //stopCreateEnemy = true;
            if (stopCreateEnemy)
                break;

            await UniTask.WaitForSeconds(gamePreferences.enemySpawnDelay);
            await UniTask.WaitUntil(() => !pause);
        }

        await UniTask.WaitUntil(() => enemiesList.Count < gamePreferences.totalEnemiesAmount);
        await UniTask.WaitUntil(() => !pause);

        if (!stopCreateEnemy)
            CreateEnemies().Forget();
    }

    public void RemoveEnemyFromList(AbstractUnit enemy)
    {
        enemiesList.Remove(enemy);

        if (enemiesList.Count == 0 && stopCreateEnemy)
        {
            EventBus<RoundCompleteEvnt>.Publish(new() { type = RoundCompleteType.Win });
        }
    }
}
