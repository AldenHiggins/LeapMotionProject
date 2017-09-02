using UnityEngine;
using UnityEngine.UI;

public class PlaceDefenseAttack : AAttack
{
    // Arrays for the defenses and their different visual forms
    [SerializeField]
    private DefensiveObject[] defenses;

    // The current game objects for our pending/invalid defensive object
    private GameObject createdDefensiveObject;
    private GameObject invalidDefensiveObject;

    // Keep track of whether the invalid/pending defensive objects have been instantiated
    private bool isInstantiated = false;

    // Keep track of the current index into the defense arrays
    private int currentDefense;

    // The pointer that shows where our controller is pointing
    [SerializeField]
    private GameObject defensivePointer;

    // The layer that we raycast to for our defenses
    private int defenseGridLayerMask = 1 << 10;

    // Keep the game around to check if the offensive mode has started to remove any pending defenses
    private GameLogic game;

    // Keep a reference to the player around to deduct gold to place defenses
    private PlayerLogic player;

    // Keep a reference to the defensive grid to correctly place objects
    private DefensiveGrid grid;

    void Start()
	{
        // Initialize the first defense
        currentDefense = -1;
        switchDefense();
        game = GetObjects.getGame();
        grid = GetObjects.getDefensiveGrid();
        player = GetObjects.getPlayer();
        // Instantiate our defensive pointer
        defensivePointer = Instantiate(defensivePointer);
        defensivePointer.transform.parent = GetObjects.getMiscContainer();
        defensivePointer.SetActive(false);
    }

    void Update()
    {
        if (game.roundActive == true)
        {
            defensivePointer.SetActive(false);
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

        // Check to see if the player has enough money
        if (player.getGold() < defenses[currentDefense].getCost())
        {
            return;
        }

        // Place the new defense if a spot is available
        if (grid.placeNewDefense(defenseLocation))
        {
            GameObject ballistaFinal = Instantiate(defenses[currentDefense].getDefensiveObject(), defenseLocation + new Vector3(0.0f, 0.1f, 0.0f), defenseRotation(localRot));
            ballistaFinal.transform.parent = GetObjects.getDefenseContainer();
            player.spendGold(defenses[currentDefense].getCost());
            destroyPendingObject();
        }
    }

    public override void holdFunctionConcrete(Vector3 localPos, Vector3 worldPos, Quaternion localRot, Quaternion worldRot)
    {
        // Show our defensive pointer in the correct place
        defensivePointer.transform.position = worldPos;
        defensivePointer.transform.rotation = worldRot;
        defensivePointer.SetActive(true);
        
        // Create our pending defensive object if it hasn't been done already
        if (!isInstantiated)
        {
            createdDefensiveObject = Instantiate(defenses[currentDefense].getPending());
            invalidDefensiveObject = Instantiate(defenses[currentDefense].getInvalid());
            isInstantiated = true;
        }

        // Get the location that we should place our defense in
        Vector3 defenseLocation = new Vector3();
        if (!raycastToPlaceDefense(worldPos, worldRot, ref defenseLocation))
        {
            return;
        }

        // Check to see if this is an invalid location or if we don't have enough money
        if (grid.isSpotTaken(defenseLocation) || player.getGold() < defenses[currentDefense].getCost())
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


