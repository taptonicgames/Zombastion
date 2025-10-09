using System;
using System.Collections.Generic;
using System.Linq;
using StarterAssets;
using UnityEngine;
using Zenject;

public class PlayerCharacter : AbstractPlayerUnit
{
    [Inject]
    private readonly DiContainer diContainer;

    [SerializeField]
    private List<AbstractWeapon> weapons;
    private ThirdPersonController thirdPersonController;

	private void Awake()
	{
        Init();
	}

	private void Start()
    {
        animator = GetComponent<Animator>();
        weapon = weapons.First();
        thirdPersonController = GetComponent<ThirdPersonController>();

        thirdPersonController.MoveSpeed = SOData.Speed;
        health = SOData.Health;

        abilitiesPair = new Dictionary<AbilityType, AbstractUnitAbility>
        {
            { AbilityType.Heal, new PlayerHealAbility(this) },
        };

        unitActionsList = new()
        {
            new UnitPauseAction(this),
            new PlayerDieAction(this),
            new PlayerAttackAction(this),
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

    public override Type GetDamageRecieverType()
    {
        return typeof(PlayerCharacter);
    }
}
