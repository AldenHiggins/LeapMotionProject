using UnityEngine;
using System.Collections;

public class BasicFireballAttack : AAttack 
{
	public GameLogic game;
	
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
		game.playerCastFireball ();
	}
	
	public override void holdGestureFunction(HandModel[] hands)
	{
		
	}
	
	public override void inactiveFunction(HandModel[] hands)
	{

	}
}
