using UnityEngine;
using System.Collections;

public class ClapAttack : AAttack 
{

	public GameObject clapProjectile;
	public GameObject player;

	public override void chargingFunction(HandModel[] hands)
	{
		
	}

	public override void chargedFunction(HandModel[] hands)
	{}
	
	public override void releaseFunction(HandModel[] hands)
	{
		Instantiate (clapProjectile, player.transform.position + new Vector3 (0.0f, 0.7f, 0.0f), Quaternion.identity);
	}
	
	public override void holdGestureFunction(HandModel[] hands)
	{
		
	}
	
	public override void inactiveFunction()
	{
		
	}
}
