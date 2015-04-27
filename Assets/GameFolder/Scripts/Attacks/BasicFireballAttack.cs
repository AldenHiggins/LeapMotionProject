using UnityEngine;
using System.Collections;

public class BasicFireballAttack : AAttack 
{
	public GameLogic game;
	public GameObject thisCamera;
	public GameObject fireBall;
	public GameObject projectileActiveParticle;


	public float firingCoolDown;

	private bool canFire;

	
	// Use this for initialization
	void Start () 
	{
		canFire = true;
	}

	
	public override void chargingFunction(HandModel[] hands)
	{

	}
	
	public override void chargedFunction(HandModel[] hands)
	{}
	
	public override void releaseFunction(HandModel[] hands)
	{
		// Check and see if the cooldown is up
		if (canFire != true)
		{
			return;
		}
		// Wait for the next cool down in order to fire again
		projectileActiveParticle.SetActive (false);
		canFire = false;
		StartCoroutine (waitForCoolDown ());

		// Have the player spend mana
		// playerLogic.useEnergy(10);
		// Make sure the fireball spawns in front of the player at a reasonable distance
		Vector3 spawnPosition = thisCamera.transform.position;
		spawnPosition += new Vector3(thisCamera.transform.forward.normalized.x * .8f, thisCamera.transform.forward.normalized.y * .8f, thisCamera.transform.forward.normalized.z * .8f);
		// Scale the fireball's velocity
		Vector3 startingVelocity = thisCamera.transform.forward.normalized;
		startingVelocity *= .2f;

		GameObject newFireball = (GameObject) Instantiate(fireBall, spawnPosition, thisCamera.transform.rotation);
		newFireball.SetActive(true); 
		MoveFireball moveThis = (MoveFireball) newFireball.GetComponent(typeof(MoveFireball));
		moveThis.setVelocity(startingVelocity);
		newFireball.GetComponent<Renderer>().enabled = true;
		moveThis.setHash (0);
				
	}
	
	public override void holdGestureFunction(HandModel[] hands)
	{
		
	}
	
	public override void inactiveFunction()
	{

	}

	IEnumerator waitForCoolDown()
	{

		yield return new WaitForSeconds (firingCoolDown);
		projectileActiveParticle.SetActive (true);
		canFire = true;
	}
}
