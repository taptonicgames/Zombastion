using System.Linq;
using UnityEngine;

public class Castle : MonoBehaviour, IColliderHelper
{
    [SerializeField]
    private CastleSO SOData;

    [field: SerializeField]
    public Gates Gates { get; private set; }

    public int Health { get; private set; }

    private const string OUTER_COLLIDER = "OuterCollider";
    private const string INNER_COLLIDER = "InnerCollider";
    private Transform inCollisionTr;

    private void Awake()
    {
        Health = SOData.Health;
        EventBus<UpgradeChoosenEvnt>.Subscribe(OnUpgradeChoosenEvnt);
        GetComponentsInChildren<AbstractPlayerUnit>(true).ToList().ForEach(a => a.Init());
    }

    private void OnUpgradeChoosenEvnt(UpgradeChoosenEvnt evnt) { }

    public void SetDamage(int damage)
    {
        Health = Mathf.Clamp(0, Health - damage, Health);
        CheckGates();
    }

    private void CheckGates()
    {
        if (Health == 0)
        {
            Gates.gameObject.SetActive(false);
            EventBus<GatesFallenEvnt>.Publish(new());
        }
    }

    public void EnterCollider(Collider collider, Transform sender, Collision collision = null)
    {
        if (collider.gameObject.layer == Constants.PLAYER_LAYER)
        {
            Gates.OpenClose(Constants.OPEN, true);
        }

        inCollisionTr = sender;
    }

    public void StayCollider(Collider collider, Transform sender, Collision collision = null) { }

    public void ExitCollider(Collider collider, Transform sender, Collision collision = null)
    {
        if (inCollisionTr == sender)
        {
            if (collider.gameObject.layer == Constants.PLAYER_LAYER)
            {
                Gates.OpenClose(Constants.OPEN, false);

                if (sender.name == INNER_COLLIDER)
                    EventBus<PlayerFindingCastleEvnt>.Publish(
                        new() { type = PlayerFindingCastleType.Entered }
                    );

                if (sender.name == OUTER_COLLIDER)
                    EventBus<PlayerFindingCastleEvnt>.Publish(
                        new() { type = PlayerFindingCastleType.Left }
                    );

                inCollisionTr = null;
            }
        }
    }

    public int GetDefaultHealth()
    {
        return SOData.Health;
    }
}
