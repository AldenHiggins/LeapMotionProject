using UnityEngine;

public class SteamFireballAttack : SteamAttacks
{
	public PlayerLogic player;
	public GameLogic game;
	public GameObject fireBall;
//	public GameObject projectileActiveParticle;
//	public int manaCost;
	
	
//	public float firingCoolDown;
	
//	private bool canFire;
	
	
	// Use this for initialization
	void Start () 
	{
//		canFire = true;
	}

	
//	IEnumerator waitForCoolDown()
//	{
//		
//		yield return new WaitForSeconds (firingCoolDown);
//		projectileActiveParticle.SetActive (true);
//		canFire = true;
//	}


	public override void inactiveFunction(){}
	
	public override void releaseFunction(uint controllerIndex, GameObject trackedDevice)
	{		
		// Have the player spend mana
		// playerLogic.useEnergy(10);
		// Make sure the fireball spawns in front of the player at a reasonable distance
		Vector3 spawnPosition = trackedDevice.transform.position;
//		spawnPosition += new Vector3(trackedDevice.transform.up.normalized.x * -.8f, trackedDevice.transform.up.normalized.y * -.8f, trackedDevice.transform.up.normalized.z * -.8f);
		// Scale the fireball's velocity
		Vector3 startingVelocity = trackedDevice.transform.up;
		startingVelocity *= -.2f;
		
		//		print ("Spawning fireball");
		GameObject newFireball = (GameObject) Instantiate(fireBall, spawnPosition, Quaternion.LookRotation((-1 * trackedDevice.transform.up)));
		newFireball.SetActive(true); 
		MoveFireball moveThis = (MoveFireball) newFireball.GetComponent(typeof(MoveFireball));
//		moveThis.setVelocity(startingVelocity);
		newFireball.GetComponent<Renderer>().enabled = true;
		moveThis.setHash (0);
	}
	
	public override void holdFunction(uint controllerIndex, GameObject trackedDevice){}
}


