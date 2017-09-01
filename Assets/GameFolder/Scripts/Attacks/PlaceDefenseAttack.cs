using UnityEngine;
using UnityEngine.UI;

public class PlaceDefenseAttack : AAttack
{
	public GameObject defensiveObject;
	public GameObject defensiveObjectPending;
    public GameObject defensiveObjectInvalid;
	private bool isInstantiated = false;
	private GameObject createdDefensiveObject;
    private GameObject invalidDefensiveObject;

    public GameObject[] defenses;
    public GameObject[] pendingDefenses;
    public GameObject[] invalidDefenses;
    private int currentDefense;

    // The layer that we raycast to for our defenses
    private int defenseGridLayerMask = 1 << 10;

    // Keep the game around to check if the offensive mode has started to remove any pending defenses
    private GameLogic game;

    // Keep a reference to the defensive grid to correctly place objects
    private DefensiveGrid grid;

    void Start()
	{
        currentDefense = 0;
        game = GetObjects.getGame();
        grid = GetObjects.getDefensiveGrid();
	}

    void Update()
    {
        if (game.roundActive == true)
        {
            destroyPendingObject();
        }
    }

    public override void inactiveFunction()
    {
        destroyPendingObject();
    }

    public override void releaseFunctionConcrete(Vector3 localPos, Vector3 worldPos, Quaternion localRot, Quaternion worldRot)
    {
        // Get the location we should place our defense in
        Vector3 defenseLocation = new Vector3();
        if (!raycastToPlaceDefense(worldPos, worldRot, ref defenseLocation))
        {
            return;
        }

        // Place the new defense if a spot is available
        if (grid.placeNewDefense(defenseLocation))
        {
            GameObject ballistaFinal = Instantiate(defensiveObject, defenseLocation + new Vector3(0.0f, 0.1f, 0.0f), defenseRotation(localRot));
            destroyPendingObject();
        }
    }

    public override void holdFunctionConcrete(Vector3 localPos, Vector3 worldPos, Quaternion localRot, Quaternion worldRot)
    {
        // Create our pending defensive object if it hasn't been done already
        if (!isInstantiated)
        {
            createdDefensiveObject = Instantiate(defensiveObjectPending);
            invalidDefensiveObject = Instantiate(defensiveObjectInvalid);
            isInstantiated = true;
        }

        // Get the location that we should place our defense in
        Vector3 defenseLocation = new Vector3();
        if (!raycastToPlaceDefense(worldPos, worldRot, ref defenseLocation))
        {
            return;
        }

        // Check to see if this is an invalid location...if so replace our defensive object with an invalid one
        if (grid.isSpotTaken(defenseLocation))
        {
            createdDefensiveObject.SetActive(false);
            invalidDefensiveObject.SetActive(true);
        }
        else
        {
            createdDefensiveObject.SetActive(true);
            invalidDefensiveObject.SetActive(false);
        }

        // Show the user where there defense will be placed
        createdDefensiveObject.transform.position = defenseLocation + new Vector3(0.0f, 0.1f, 0.0f);
        createdDefensiveObject.transform.rotation = defenseRotation(localRot);
        invalidDefensiveObject.transform.position = defenseLocation + new Vector3(0.0f, 0.1f, 0.0f);
        invalidDefensiveObject.transform.rotation = defenseRotation(localRot);
    }

    private Quaternion defenseRotation(Quaternion localRot)
    {
        //return Quaternion.Euler(0.0f, -1 * localRot.eulerAngles.z, 0.0f);
        return Quaternion.identity;
    }

    private bool raycastToPlaceDefense(Vector3 worldPos, Quaternion worldRot, ref Vector3 defensePosition)
    {
        // Raycast onto the defensive grid
        RaycastHit hit;
        Physics.Raycast(worldPos, (worldRot * Vector3.forward), out hit, 100.0f, defenseGridLayerMask);

        // Make sure the raycast hit the defensive grid
        if (hit.collider == null)
        {
            return false;
        }
        if (hit.collider.gameObject == null)
        {
            return false;
        }

        // Set the defensive position
        defensePosition = grid.getClosestGridPoint(hit.point);

        return true;
    }

    public void switchDefense()
	{
        currentDefense++;
        destroyPendingObject();

        if (currentDefense >= defenses.Length)
        {
            currentDefense = 0;
        }

        defensiveObject = defenses[currentDefense];
        defensiveObjectPending = pendingDefenses[currentDefense];
        defensiveObjectInvalid = invalidDefenses[currentDefense];
    }

    void destroyPendingObject()
    {
        if (isInstantiated)
        {
            isInstantiated = false;
            Destroy(createdDefensiveObject);
            Destroy(invalidDefensiveObject);
        }
    }
}


