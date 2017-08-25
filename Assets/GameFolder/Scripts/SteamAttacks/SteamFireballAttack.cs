using UnityEngine;

public class SteamFireballAttack : SteamAttacks
{
	public GameObject fireBall;

    // Keep track of the player's transform
    private Transform playerRoot;

    void Start()
    {
        playerRoot = GetObjects.getPlayer().gameObject.transform.parent;
    }

    public override void inactiveFunction() { }

    public override void releaseFunction(OVRInput.Controller hand)
    {
        // Generate a fireball at the controller that fires out from the correct angle (so that the controller is a gun)
        Vector3 spawnPosition = playerRoot.TransformPoint(OVRInput.GetLocalControllerPosition(hand));

        Quaternion newRotation = playerRoot.rotation * OVRInput.GetLocalControllerRotation(hand);
        //newRotation *= Quaternion.AngleAxis(45, Vector3.right);

        GameObject newFireball = (GameObject)Instantiate(fireBall, spawnPosition, newRotation);
        newFireball.SetActive(true);
        newFireball.GetComponent<Renderer>().enabled = true;
    }

    public override void holdFunction(OVRInput.Controller hand) { }
}


