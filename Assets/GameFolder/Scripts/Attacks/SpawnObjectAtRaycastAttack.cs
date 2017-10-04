using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectAtRaycastAttack : AAttack
{
    public GameObject objectToSpawn;

    private int groundLayer = 1 << 10;

    private PlayerLogic player;

    private Vector3 currentSpawnPosition;

    private void Start()
    {
        player = GetObjects.instance.getPlayer();
    }

    public override void inactiveFunction() { }

    public override void releaseFunctionConcrete(Vector3 localPos, Vector3 worldPos, Quaternion localRot, Quaternion worldRot)
    {
        // Hide the pointer
        player.hidePointers();

        // Get access to the unit
        ControllableUnit unit = GetObjects.instance.getControllableUnit();

        // Check that the unit isn't already attacking
        if (unit.getAttacking()) return;

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

        // Set the spawn position for the object to be our raycast point
        currentSpawnPosition = hit.point;
        // Have the unit face the point we're casting towards
        unit.transform.LookAt(currentSpawnPosition, Vector3.up);
        // Have the unit cast this spell
        unit.castSpell(createObjectOnCast);
    }

    public override void holdFunctionConcrete(Vector3 localPos, Vector3 worldPos, Quaternion localRot, Quaternion worldRot)
    {
        player.showPointer(false);
    }

    private void createObjectOnCast()
    {
        Instantiate(objectToSpawn, currentSpawnPosition, Quaternion.identity, GetObjects.instance.getAttackParticleContainer());
    }
}
