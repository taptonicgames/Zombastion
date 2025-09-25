using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class RagdollHandler : MonoBehaviour
{
    public float massDecrement = 1;
    public float drag = 0;
    public Transform spine, boneFolder;
    [HideInInspector] public List<Rigidbody> rigidbodiesList;
    [HideInInspector] public bool isRagdollActive, isAnimatorActive;
    [HideInInspector] public Transform parentFolder;
    private List<Collider> collidersList;
    private bool isStartDone;
    private Vector3 defaultBodyPos;

    private void Start()
    {
        rigidbodiesList = GetComponentsInChildren<Rigidbody>().ToList();
        collidersList = GetComponentsInChildren<Collider>().ToList();
        rigidbodiesList.Remove(GetComponent<Rigidbody>());
        collidersList.Remove(GetComponent<Collider>());
        defaultBodyPos = boneFolder.GetChild(0).transform.localPosition;
        parentFolder = transform.parent;

        EnableRagdoll(false);
        isStartDone = true;
        //EnableRagdoll(true);
    }

    public void EnableRagdoll(bool b)
    {
        isRagdollActive = b;

        if (b)
        {
            GetComponent<Animator>().enabled = !b;
            isAnimatorActive = !b;
		}

        foreach (var item in rigidbodiesList)
        {
            item.isKinematic = !b;
            if (!isStartDone)
            {
                item.mass /= massDecrement;
            }
        }

        SetDrag(drag);
    }

    public void EnableAnimator()
    {
        EnableRagdoll(false);
        transform.position = spine.position;
        GetComponent<Animator>().enabled = true;
        isRagdollActive = false;
        isAnimatorActive = true;
        boneFolder.GetChild(0).transform.localRotation = Quaternion.identity;
        boneFolder.GetChild(0).transform.localPosition = defaultBodyPos;
        GetComponent<Collider>().enabled = true;
    }

    public void EnableGravity(bool b)
    {
        foreach (var item in rigidbodiesList)
        {
            item.useGravity = b;
        }
    }

    public void SetDrag(float f)
    {
        foreach (var item in rigidbodiesList)
        {
            item.drag = f;
        }
    }

    public bool IsOnNavMesh(float radius)
    {
        NavMeshHit navMeshHit = new NavMeshHit();
        var id = GetComponent<NavMeshAgent>().agentTypeID;
        NavMeshQueryFilter queryFilter = new NavMeshQueryFilter { agentTypeID = id, areaMask = NavMesh.AllAreas };
        var v = NavMesh.SamplePosition(spine.position, out navMeshHit, radius, queryFilter);
        return v;
    }

    public bool IsOnNavMesh(float radius, out NavMeshHit navMeshHit)
    {
        var id = GetComponent<NavMeshAgent>().agentTypeID;
        NavMeshQueryFilter queryFilter = new NavMeshQueryFilter { agentTypeID = id, areaMask = NavMesh.AllAreas };
        var v = NavMesh.SamplePosition(spine.position, out navMeshHit, radius, queryFilter);
        return v;
    }

    public bool IsOnNavMesh(Vector3 pos, float radius)
    {
        NavMeshHit navMeshHit = new NavMeshHit();
        var id = GetComponent<NavMeshAgent>().agentTypeID;
        NavMeshQueryFilter queryFilter = new NavMeshQueryFilter { agentTypeID = id, areaMask = NavMesh.AllAreas };
        var v = NavMesh.SamplePosition(pos, out navMeshHit, radius, queryFilter);
        return v;
    }

    public Transform GetSurface()
    {
        var pos = spine.position;
        pos.y = 100;
        Ray ray = new Ray(pos, Vector3.down);
        RaycastHit raycastHit = new RaycastHit();
        int mask = 1 << 7;
        int mask1 = 1 << 9;
        mask = mask | mask1;
        //mask = ~mask;
        Physics.Raycast(ray, out raycastHit, 150, mask);
        return raycastHit.collider.transform;
    }

    public Rigidbody GetRigidbodyWithTag(string tag)
    {
        return rigidbodiesList.Find(a => a.tag == tag);
    }

    public void SetExplosion(float explosionForce, Vector3 explosionPosition, float explosionRadius, ForceMode forceMode)
    {
        foreach (var item in rigidbodiesList)
        {
            item.AddExplosionForce(explosionForce, explosionPosition, explosionRadius, 0, forceMode);
        }
    }
}