using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class CharacterFactory
{
    [Inject]
    private readonly SceneReferences sceneReferences;

    [Inject]
    private readonly SharedObjects sharedObjects;

    [Inject]
    DiContainer diContainer;
    private const float Y_SPAWN_POSITION = 0;

    public IEnumerable<AbstractUnit> Spawn(
        CharacterType characterType,
        int amount,
        Collider spawnArea
    )
    {
        var bounds = spawnArea.bounds;
        var folder = sceneReferences.charactersFolder;
        var prefab = sharedObjects.GetPrefab(characterType.ToString());
        List<AbstractUnit> charactersList = new();

        for (int i = 0; i < amount; i++)
        {
            var posX = Random.Range(bounds.min.x, bounds.max.x);
            var posZ = Random.Range(bounds.min.z, bounds.max.z);

            NavMesh.SamplePosition(
                new Vector3(posX, Y_SPAWN_POSITION, posZ),
                out var hit,
                10,
                NavMesh.AllAreas
            );

            var rot = Quaternion.Euler(0, Random.Range(0, 360), 0);

            //var character = Object
            //    .Instantiate(prefab, hit.position, rot, folder)
            //    .GetComponent<AbstractUnit>();

            var character = diContainer.InstantiatePrefabForComponent<AbstractUnit>(
                prefab,
                hit.position,
                rot,
                folder
            );

            charactersList.Add(character);
            character.transform.SetParent(folder);
            character.Init();
        }

        return charactersList;
    }
}
