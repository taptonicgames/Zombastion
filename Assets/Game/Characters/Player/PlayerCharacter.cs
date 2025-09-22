using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerCharacter : AbstractUnit
{
    [Inject]
    DiContainer diContainer;

    [SerializeField]
    private List<AbstractWeapon> weapons;

    private void Start()
    {
        abilitiesPair = new Dictionary<AbilityType, AbstractUnitAbility>
        {
            {
                AbilityType.Attack,
                new UnitAttackAbility(this, new() { CharacterType.SimpleZombie })
            },
        };

        unitActionsList = new()
        {
            new UnitAttackAction(this),
            new PlayerMoveAction(this),
            new UnitIdleAction(this),
        };

        foreach (var item in unitActionsList)
        {
            diContainer.Inject(item);
        }
    }

    protected override void Update()
    {
        foreach (var item in abilitiesPair)
        {
            item.Value.Update();
        }
        if (!IsEnable)
            return;

        for (int i = 0; i < unitActionsList.Count; i++)
        {
            if (unitActionsList[i].CheckAction())
                break;
        }

        if (UnitAction != null)
            UnitAction.Update();
    }
}
