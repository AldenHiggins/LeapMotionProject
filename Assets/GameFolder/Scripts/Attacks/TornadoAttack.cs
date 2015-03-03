using UnityEngine;
using System.Collections;

public class TornadoAttack : AAttack 
{

	public PlayerLogic player;
	public GameObject tornadoParticle;
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	public override void chargingFunction(HandModel[] hands)
	{
		
	}
	
	public override void chargedFunction(HandModel[] hands)
	{}
	
	public override void releaseFunction(HandModel[] hands)
	{
		Instantiate (tornadoParticle, player.transform.position, player.transform.rotation);
	}
	
	public override void holdGestureFunction(HandModel[] hands)
	{
		
	}
	
	public override void inactiveFunction(HandModel[] hands)
	{
		
	}
}
