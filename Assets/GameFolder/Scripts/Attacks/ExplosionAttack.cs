using UnityEngine;
using System.Collections;

public class ExplosionAttack : AAttack
{
	public GameObject explosion;
	public PlayerLogic player;

	public override void chargingFunction(HandModel[] hands)
	{

	}
	
	public override void chargedFunction(HandModel[] hands)
	{
		// DEACTIVATE ABILITY
	}
	
	public override void releaseFunction(HandModel[] hands)
	{
		GameObject createdExplosion = (GameObject) Instantiate (explosion, player.transform.position, player.transform.rotation);
		createdExplosion.SetActive (true);
	}
	
	public override void holdGestureFunction(HandModel[] hands)
	{
		
	}
	
	public override void inactiveFunction()
	{

	}
}


