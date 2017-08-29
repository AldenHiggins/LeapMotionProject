using UnityEngine;
using UnityEngine.UI;

public class PlaceDefenseAttack : AAttack
{
	public GameObject defensiveObject;
	public GameObject defensiveObjectPending;
	private bool isInstantiated = false;
	private GameObject createdDefensiveObject;

    public GameObject[] defenses;
    public GameObject[] pendingDefenses;
    private int currentDefense;

    // Keep the game around to check if the offensive mode has started to remove any pending defenses
    private GameLogic game;

    // Keep track of the player's transform
    private Transform playerRoot;

	void Start()
	{
        currentDefense = 0;
        game = GetObjects.getGame();
        playerRoot = GetObjects.getPlayer().gameObject.transform.parent;
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
        int layerMask = 1 << 10;
        RaycastHit hit;
        Physics.Raycast(worldPos, (worldRot * Vector3.forward), out hit, 100.0f, layerMask);

        // Make sure the raycast hit something
        if (hit.collider == null)
        {
            return;
        }
        if (hit.collider.gameObject == null)
        {
            return;
        }

        GameObject ballistaFinal = (GameObject)Instantiate(defensiveObject, hit.point + new Vector3(0.0f, 0.1f, 0.0f), Quaternion.Euler(0.0f, -1 * localRot.eulerAngles.z, 0.0f));
        ballistaFinal.SetActive(true);
        destroyPendingObject();
    }

    public override void holdFunctionConcrete(Vector3 localPos, Vector3 worldPos, Quaternion localRot, Quaternion worldRot)
    {
        if (!isInstantiated)
        {
            createdDefensiveObject = Instantiate(defensiveObjectPending);
            createdDefensiveObject.SetActive(true);
            isInstantiated = true;
        }

        int layerMask = 1 << 10;
        RaycastHit hit;
        Physics.Raycast(worldPos, (worldRot * Vector3.forward), out hit, 100.0f, layerMask);

        // Make sure the raycast hit something
        if (hit.collider == null)
        {
            return;
        }
        if (hit.collider.gameObject == null)
        {
            return;
        }

        createdDefensiveObject.transform.position = hit.point + new Vector3(0.0f, 0.1f, 0.0f);
        createdDefensiveObject.transform.rotation = Quaternion.Euler(0.0f, -1 * localRot.eulerAngles.z, 0.0f);
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
	}

    void destroyPendingObject()
    {
        if (isInstantiated)
        {
            isInstantiated = false;
            Destroy(createdDefensiveObject);
        }
    }
}


