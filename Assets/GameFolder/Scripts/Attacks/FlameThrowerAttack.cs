using UnityEngine;
using System.Collections;

public class FlameThrowerAttack : AAttack
{
	public GameObject flameThrowerParticle;
	public float positionOffset;
	public AudioClip flameThrowerNoise;
	
	private GameObject[] flamethrowers = new GameObject[2];
	
	// Use this for initialization
	void Start () 
	{
		flamethrowers[0] = (GameObject) Instantiate(flameThrowerParticle);
		flamethrowers[1] = (GameObject) Instantiate(flameThrowerParticle);
		flamethrowers[0].SetActive (false);
		flamethrowers[1].SetActive (false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	public override void chargingFunction(HandModel[] hands)
	{
		//		MoveFireball fireball = (MoveFireball) flameThrowerParticle.GetComponent (typeof(MoveFireball));
		//		fireball.stopPeriodicDamage();
		//		flameThrowerParticle.SetActive (false);
	}
	
	public override void chargedFunction(HandModel[] hands)
	{
		//		flameThrowerParticle.SetActive (true);
		//		MoveFireball fireball = (MoveFireball) flameThrowerParticle.GetComponent (typeof(MoveFireball));
		//		fireball.startPeriodicDamage();
		
	}
	
	public override void releaseFunction(HandModel[] hands)
	{
		if (flamethrowers [0] == null || flamethrowers[1] == null)
			return;
		
		flamethrowers[0].SetActive (true);
		flamethrowers[1].SetActive (true);
		MoveFireball fireball1 = (MoveFireball) flamethrowers[0].GetComponent (typeof(MoveFireball));
		MoveFireball fireball2 = (MoveFireball) flamethrowers[1].GetComponent (typeof(MoveFireball));
		fireball1.startPeriodicDamage();
		fireball2.startPeriodicDamage();
		
		// Play the flamethrower sound
		AudioSource source = flamethrowers [0].GetComponent<AudioSource> ();
		source.PlayOneShot (flameThrowerNoise);
	}
	
	public override void holdGestureFunction(HandModel[] hands)
	{
		if (flamethrowers [0] == null || flamethrowers[1] == null)
			return;
		if (hands.Length > 0)
		{
			if (!flameThrowerParticle.activeSelf)
			{
				//print ("Flamethrower not active!");
			}
			
			for(int i = 0; i < hands.Length && i < 2; i++) 
			{
				flamethrowers[i].transform.position = hands[i].GetPalmPosition();
				flamethrowers[i].transform.position += (positionOffset * hands[i].GetPalmNormal());
				flamethrowers[i].transform.rotation = Quaternion.LookRotation(hands[i].GetPalmNormal());
			}
		}		
	}
	
	public override void inactiveFunction()
	{
		if (flamethrowers [0] == null || flamethrowers[1] == null)
			return;
		MoveFireball fireball1 = (MoveFireball) flamethrowers[0].GetComponent (typeof(MoveFireball));
		MoveFireball fireball2 = (MoveFireball) flamethrowers[1].GetComponent (typeof(MoveFireball));
		fireball1.stopPeriodicDamage();
		fireball2.stopPeriodicDamage();
		
		//		MoveFireball fireball = (MoveFireball) flameThrowerParticle.GetComponent (typeof(MoveFireball));
		//		fireball.stopPeriodicDamage();
		flamethrowers[0].SetActive (false);
		flamethrowers[1].SetActive (false);
	}
}