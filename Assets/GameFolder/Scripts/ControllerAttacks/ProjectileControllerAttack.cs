using System;
using UnityEngine;

public class ProjectileControllerAttack : AControllerAttack
{
	public MoveFireball fireBall;

	public override void chargingFunction(){}
	
	public override void chargedFunction(){}
	
	public override void releaseFunction(GameObject camera)
	{
		// Have the player spend mana
		// playerLogic.useEnergy(10);
		// Make sure the fireball spawns in front of the player at a reasonable distance
		Vector3 spawnPosition = camera.transform.position;
		spawnPosition += new Vector3(camera.transform.forward.normalized.x * .8f, camera.transform.forward.normalized.y * .8f, camera.transform.forward.normalized.z * .8f);
		// Scale the fireball's velocity
		Vector3 startingVelocity = camera.transform.forward.normalized;
		startingVelocity *= .2f;
		
		//		print ("Spawning fireball");
		GameObject newFireball = (GameObject) Instantiate(fireBall.gameObject, spawnPosition, camera.transform.rotation);
		newFireball.SetActive(true); 
		MoveFireball moveThis = (MoveFireball) newFireball.GetComponent(typeof(MoveFireball));
		moveThis.setVelocity(startingVelocity);
		newFireball.GetComponent<Renderer>().enabled = true;
		moveThis.setHash (0);
	}
	
	public override void holdFunction(){}
	
	public override void inactiveFunction(){}
}


