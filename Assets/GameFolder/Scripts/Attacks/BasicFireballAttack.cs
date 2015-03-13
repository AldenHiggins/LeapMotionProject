using UnityEngine;
using System.Collections;

public class BasicFireballAttack : AAttack 
{
	public GameLogic game;
	public GameObject thisCamera;
	public GameObject fireBall;
	
	// Use this for initialization
	void Start () 
	{
	}

	
	public override void chargingFunction(HandModel[] hands)
	{

	}
	
	public override void chargedFunction(HandModel[] hands)
	{}
	
	public override void releaseFunction(HandModel[] hands)
	{
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
	
	public override void inactiveFunction(HandModel[] hands)
	{

	}
}
