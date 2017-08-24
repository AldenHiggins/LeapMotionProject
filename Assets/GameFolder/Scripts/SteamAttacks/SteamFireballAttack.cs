using UnityEngine;

public class SteamFireballAttack : SteamAttacks
{
	public PlayerLogic player;
	public GameLogic game;
	public GameObject fireBall;

	public override void inactiveFunction(){}
	
	public override void releaseFunction(uint controllerIndex, SteamVR_TrackedObject trackedDevice)
	{		
		// Generate a fireball at the controller that fires out from the correct angle (so that the controller is a gun)
		Vector3 spawnPosition = trackedDevice.transform.position;

        Quaternion newRotation = trackedDevice.transform.rotation;
        newRotation *= Quaternion.AngleAxis(45, Vector3.right);

        GameObject newFireball = (GameObject)Instantiate(fireBall, spawnPosition, newRotation);
        newFireball.SetActive(true); 
		newFireball.GetComponent<Renderer>().enabled = true;
	}
	
	public override void holdFunction(uint controllerIndex, SteamVR_TrackedObject trackedDevice){}
}


