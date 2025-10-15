using System;
using UnityEngine;

public class Tower : AbstractPlayerUnit
{
    [SerializeField]
    private AbstractWeapon towerWeapon;

    public override void Init()
    {
        base.Init();
        weapon = towerWeapon;
        EventBus<UpgradeChoosenEvnt>.Subscribe(OnUpgradeChoosenEvnt);
    }

    private void Start()
    {
        unitActionsList = new() { new TowerAttackAction(this), new UnitIdleAction(this) };

        foreach (var item in unitActionsList)
        {
            diContainer.Inject(item);
        }
    }

    private void OnUpgradeChoosenEvnt(UpgradeChoosenEvnt evnt)
    {
        if (evnt.type != BattleUpgradeType.TowerBuild)
            return;
        if (!weapon)
            return;
        if (evnt.config.WeaponType != towerWeapon.WeaponSOData.WeaponType)
            return;
        gameObject.SetActive(true);
    }

    public override Type GetDamageRecieverType()
    {
        return typeof(Tower);
    }

    protected override void OnSetGamePauseEvnt(SetGamePauseEvnt evnt)
    {
        //base.OnSetGamePauseEvnt(evnt);
    }
}
