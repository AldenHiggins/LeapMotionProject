using UnityEngine;


public class SpawnSkeletonAttack : AAttack
{
	public GameObject zombiePrefab;
    public GameObject aoeIndicator;
    public float castRange;
    public float corpseFindRange;
    private int groundLayerMask;
    private PlayerLogic player;
    private Vector3 currentCastPosition;
    private int corpseLayerMask;

    private void Start()
    {
        corpseLayerMask = LayerMask.GetMask("Corpses");
        groundLayerMask = LayerMask.GetMask("Ground");
        player = GetObjects.instance.getPlayer();
        aoeIndicator = Instantiate(aoeIndicator, GetObjects.instance.getAttackContainer());
        aoeIndicator.SetActive(false);
    }

    public override void inactiveFunction(){}

    public override void releaseFunctionConcrete(Vector3 localPos, Vector3 worldPos, Quaternion localRot, Quaternion worldRot)
    {
        // Hide the pointer and aoe indicator
        player.hidePointers();
        aoeIndicator.SetActive(false);

        // Get access to the unit
        ControllableUnit unit = GetObjects.instance.getControllableUnit();

        // Check that the unit isn't already attacking
        if (unit.getAttacking()) return;

        // Raycast onto the defensive grid
        Vector3 raycastPoint;
        if (!raycastTheGround(worldPos, worldRot, out raycastPoint)) return;

        // Set the spawn position for the object to be our raycast point
        currentCastPosition = raycastPoint;
        // Have the unit face the point we're casting towards
        unit.transform.LookAt(currentCastPosition, Vector3.up);
        // Have the unit cast this spell
        unit.castSpell(spawnCorpseObjectsOnCast);
    }

    private void spawnCorpseObjectsOnCast()
    {
        RaycastHit[] corpseHits = Physics.SphereCastAll(currentCastPosition, corpseFindRange, new Vector3(1.0f, 0.0f, 0.0f), 0.0f, corpseLayerMask);
        for (int corpseIndex = 0; corpseIndex < corpseHits.Length; corpseIndex++)
        {
            Instantiate(zombiePrefab, corpseHits[corpseIndex].collider.gameObject.transform.position, Quaternion.identity, corpseHits[corpseIndex].collider.gameObject.transform.parent);
            Destroy(corpseHits[corpseIndex].collider.gameObject);
        }
    }

    public override void holdFunctionConcrete(Vector3 localPos, Vector3 worldPos, Quaternion localRot, Quaternion worldRot)
    {
        player.showPointer(false);
        // Raycast onto the defensive grid to place a spell cast indicator
        Vector3 raycastPoint;
        if (!raycastTheGround(worldPos, worldRot, out raycastPoint))
        {
            aoeIndicator.SetActive(false);
            return;
        }

        aoeIndicator.transform.position = raycastPoint + new Vector3(0.0f, 0.05f, 0.0f);
        aoeIndicator.SetActive(true);
    }

    private bool raycastTheGround(Vector3 worldPos, Quaternion worldRot, out Vector3 raycastPoint)
    {
        RaycastHit hit;
        Physics.Raycast(worldPos, (worldRot * Vector3.forward), out hit, 100.0f, groundLayerMask);

        // Make sure the raycast hit the defensive grid
        if (hit.collider == null)
        {
            raycastPoint = Vector3.zero;
            return false;
        }
        if (hit.collider.gameObject == null)
        {
            raycastPoint = Vector3.zero;
            return false;
        }

        // Check if the cast is within range, if so just return the raycast point
        Vector3 unitPosition = GetObjects.instance.getControllableUnit().transform.position;
        Vector3 vectorToRaycastPoint = hit.point - unitPosition;
        if (vectorToRaycastPoint.magnitude < castRange)
        {
            raycastPoint = hit.point;
            return true;
        }
        // If the cast is out of range show the max range spot that the spell can reach
        raycastPoint = unitPosition + (vectorToRaycastPoint.normalized * castRange);
        return true;
    }
}


