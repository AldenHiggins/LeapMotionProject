using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectAtRaycastAttack : AAttack
{
    public GameObject objectToSpawn;

    private int groundLayer = 1 << 10;

    private PlayerLogic player;

    private void Start()
    {
        player = GetObjects.instance.getPlayer();
    }

    public override void inactiveFunction() { }

    public override void releaseFunctionConcrete(Vector3 localPos, Vector3 worldPos, Quaternion localRot, Quaternion worldRot)
    {
        // Hide the pointer
        player.hidePointers();

        // Raycast onto the defensive grid
        RaycastHit hit;
        Physics.Raycast(worldPos, (worldRot * Vector3.forward), out hit, 100.0f, groundLayer);

        // Make sure the raycast hit the defensive grid
        if (hit.collider == null)
        {
            return;
        }
        if (hit.collider.gameObject == null)
        {
            return;
        }

        Instantiate(objectToSpawn, hit.point, Quaternion.identity, GetObjects.instance.getAttackParticleContainer());
    }

    public override void holdFunctionConcrete(Vector3 localPos, Vector3 worldPos, Quaternion localRot, Quaternion worldRot)
    {
        player.showPointer(false);
    }
}
