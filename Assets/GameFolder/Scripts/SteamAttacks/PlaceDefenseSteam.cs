using UnityEngine;
using UnityEngine.UI;

public class PlaceDefenseSteam : SteamAttacks
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

    public override void releaseFunction(OVRInput.Controller hand)
    {
        Vector3 spawnPosition = playerRoot.TransformPoint(OVRInput.GetLocalControllerPosition(hand));

        Quaternion newRotation = playerRoot.rotation * OVRInput.GetLocalControllerRotation(hand);
        //newRotation *= Quaternion.AngleAxis(45, Vector3.right);

        Vector3 forwardVector = newRotation * Vector3.forward;

        int layerMask = 1 << 10;
        RaycastHit hit;
        Physics.Raycast(spawnPosition, forwardVector, out hit, 100.0f, layerMask);

        // Make sure the raycast hit something
        if (hit.collider == null)
        {
            return;
        }
        if (hit.collider.gameObject == null)
        {
            return;
        }

        Quaternion rotation = OVRInput.GetLocalControllerRotation(hand);
        GameObject ballistaFinal = (GameObject)Instantiate(defensiveObject, hit.point + new Vector3(0.0f, 0.1f, 0.0f), Quaternion.Euler(0.0f, -1 * rotation.eulerAngles.z, 0.0f));
        ballistaFinal.SetActive(true);
        destroyPendingObject();
    }

    public override void holdFunction(OVRInput.Controller hand)
    {
        if (!isInstantiated)
        {
            createdDefensiveObject = Instantiate(defensiveObjectPending);
            createdDefensiveObject.SetActive(true);
            isInstantiated = true;
        }

        Vector3 spawnPosition = playerRoot.TransformPoint(OVRInput.GetLocalControllerPosition(hand));

        Quaternion newRotation = playerRoot.rotation * OVRInput.GetLocalControllerRotation(hand);
        //newRotation *= Quaternion.AngleAxis(45, Vector3.right);

        Vector3 forwardVector = newRotation * Vector3.forward;

        int layerMask = 1 << 10;
        RaycastHit hit;
        Physics.Raycast(spawnPosition, forwardVector, out hit, 100.0f, layerMask);

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
        Quaternion rotation = OVRInput.GetLocalControllerRotation(hand);
        createdDefensiveObject.transform.rotation = Quaternion.Euler(0.0f, -1 * rotation.eulerAngles.z, 0.0f);
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


