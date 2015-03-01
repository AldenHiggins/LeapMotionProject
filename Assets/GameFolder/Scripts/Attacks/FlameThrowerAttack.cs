using UnityEngine;
using System.Collections;

public class FlameThrowerAttack : AAttack
{
	public GameObject flameThrowerParticle;

	// Use this for initialization
	void Start () 
	{
		Instantiate (flameThrowerParticle);
		flameThrowerParticle.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public override void chargingFunction(HandModel[] hands)
	{
		inactiveFunction (hands);
	}
	
	public override void chargedFunction(HandModel[] hands)
	{
		flameThrowerParticle.SetActive (true);
		MoveFireball fireball = (MoveFireball) flameThrowerParticle.GetComponent (typeof(MoveFireball));
		fireball.startPeriodicDamage();

	}
	
	public override void releaseFunction(HandModel[] hands)
	{
		flameThrowerParticle.SetActive (true);
		MoveFireball fireball = (MoveFireball) flameThrowerParticle.GetComponent (typeof(MoveFireball));
		fireball.startPeriodicDamage();
	}
	
	public override void holdGestureFunction(HandModel[] hands)
	{
		if (hands.Length > 0)
		{
			if (!flameThrowerParticle.activeSelf)
			{
				print ("Flamethrower not active!");
			}
			flameThrowerParticle.transform.position = hands[0].GetPalmPosition();
			flameThrowerParticle.transform.rotation = Quaternion.LookRotation(hands[0].GetPalmNormal());
		}

	}
	
	public override void inactiveFunction(HandModel[] hands)
	{
		MoveFireball fireball = (MoveFireball) flameThrowerParticle.GetComponent (typeof(MoveFireball));
		fireball.stopPeriodicDamage();
		flameThrowerParticle.SetActive (false);
	}
}
