using DG.Tweening;
using UnityEngine;

public class Castle : MonoBehaviour, IColliderHelper
{
    [SerializeField]
    private CastleSO SOData;

    [field: SerializeField]
    public Transform Gates { get; private set; }

    public int Health { get; private set; }

    private Vector3 defaultGatesPos;
    private const float GATES_OPEN_DURATION = 0.5f;
    private const string OUTER_COLLIDER = "OuterCollider";
    private const string INNER_COLLIDER = "InnerCollider";
    private Transform inCollisionTr;

    private void Awake()
    {
        Health = SOData.Health;
        defaultGatesPos = Gates.position;
    }

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
            EventBus<GatesFallenEvnt>.Publish(new GatesFallenEvnt());
        }
    }

    public void EnterCollider(Collider collider, Transform sender, Collision collision = null)
    {
        if (collider.gameObject.layer == Constants.PLAYER_LAYER)
        {
            Gates.DOMoveY(defaultGatesPos.y + 3, GATES_OPEN_DURATION);
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
                Gates.DOMoveY(defaultGatesPos.y, GATES_OPEN_DURATION);

                if (sender.name == INNER_COLLIDER)
                    EventBus<PlayerFindingCastleEvnt>.Publish(
                        new PlayerFindingCastleEvnt() { type = PlayerFindingCastleType.Entered }
                    );

                if (sender.name == OUTER_COLLIDER)
                    EventBus<PlayerFindingCastleEvnt>.Publish(
                        new PlayerFindingCastleEvnt() { type = PlayerFindingCastleType.Left }
                    );

                inCollisionTr = null;
            }
        }
    }
}
