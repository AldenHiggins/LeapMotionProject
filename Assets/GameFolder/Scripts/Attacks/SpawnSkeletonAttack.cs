using UnityEngine;


public class SpawnSkeletonAttack : AAttack
{
	public GameObject zombiePrefab;
    private PlayerLogic player;

    private void Start()
    {
        player = GetObjects.instance.getPlayer();
    }

    private int corpseLayerMask = 1 << 20;

    public override void inactiveFunction(){}

    public override void releaseFunctionConcrete(Vector3 localPos, Vector3 worldPos, Quaternion localRot, Quaternion worldRot)
    {
        // Hide the pointer
        player.hidePointers();

        // Raycast onto the defensive grid
        RaycastHit hit;
        Physics.Raycast(worldPos, (worldRot * Vector3.forward), out hit, 100.0f, corpseLayerMask);

        // Make sure the raycast hit the defensive grid
        if (hit.collider == null)
        {
            return;
        }
        if (hit.collider.gameObject == null)
        {
            return;
        }

        Instantiate(zombiePrefab, hit.collider.gameObject.transform.position, Quaternion.identity, hit.collider.gameObject.transform.parent);
        Destroy(hit.collider.gameObject);
    }

    public override void holdFunctionConcrete(Vector3 localPos, Vector3 worldPos, Quaternion localRot, Quaternion worldRot)
    {
        player.showPointer(false);
    }
}


