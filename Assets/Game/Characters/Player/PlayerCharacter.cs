using System;
using System.Collections.Generic;
using System.Linq;
using StarterAssets;
using UnityEngine;

public class PlayerCharacter : AbstractPlayerUnit
{
    [SerializeField]
    private Animator ponyAnimator;

    [SerializeField]
    private List<AbstractWeapon> weapons;
    private ThirdPersonController thirdPersonController;

    public override int Health
    {
        get => playerCharacterModel.Health.Value;
        set
        {
            playerCharacterModel.Health.Value = Mathf.Clamp(value, 0, SOData.Health);
            health = playerCharacterModel.Health.Value;
        }
    }

    private void Awake()
    {
        Init();
        playerCharacterModel.Health.Value = SOData.Health;
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
            {
                AbilityType.PonyMovement,
                new PonyMovementAbility(this, thirdPersonController, ponyAnimator)
            },
        };

        unitActionsList = new()
        {
            new UnitPauseAction(this),
            new PlayerDieAction(this),
            new PlayerMoveAction(this),
            new PlayerAttackAction(this),
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

    public virtual int HealthResurectionOnBase => SOData.HealthResurectionPerSecond;

    public override Type GetDamageRecieverType()
    {
        return typeof(PlayerCharacter);
    }

    public override void OnUnitDied() { }
}
