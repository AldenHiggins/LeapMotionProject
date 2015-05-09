using UnityEngine;
using System.Collections;

public class SwordSweepAttack : AAttack
{
	public GameObject swordObject;
	public float swordYOffset;
	public float swordAwayOffset;
	private float oldSwordAwayOffset;
	public float swordActiveDuration;
	public GameObject playerCamera;
	private bool attacking = false;

	public float yRotationalVelocity;
	private float yRotation = 0;

	public float outVelocity;
	
	// Use this for initialization
	void Start () 
	{
		oldSwordAwayOffset = swordAwayOffset;
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
//		swordObject.SetActive (true);
	}
	
	public override void releaseFunction(HandModel[] hands)
	{
//		swordObject.transform.position = hands [0].GetPalmPosition();
//		swordObject.transform.rotation = hands [0].GetPalmRotation();
//		swordObject.transform.RotateAround (swordObject.transform.position, new Vector3(0.0f, 0.0f, 1.0f), 270);
//		swordObject.transform.position += swordObject.transform.rotation * new Vector3 (0.0f, swordYOffset, 0.0f);
		if (!attacking)
		{
			attacking = true;
			swordObject.SetActive (true);
			StartCoroutine(swordArc());
			StartCoroutine(arcCooldown());
		}

	}
	
	public override void holdGestureFunction(HandModel[] hands)
	{
//		swordObject.transform.position = hands [0].GetPalmPosition();
//		swordObject.transform.rotation = hands [0].GetPalmRotation();
//		swordObject.transform.RotateAround (swordObject.transform.position, new Vector3(0.0f, 0.0f, 1.0f), 270);
//		swordObject.transform.position += swordObject.transform.rotation * new Vector3 (0.0f, swordYOffset, 0.0f);
	}
	
	public override void inactiveFunction()
	{
//		swordObject.SetActive (false);
	}

	IEnumerator swordArc()
	{
		while (true)
		{
			yield return new WaitForSeconds(.02f);
			if (attacking == false)
			{
				yield break;
			}
			print ("Throwing out sword!");
			swordAwayOffset += outVelocity;
			Vector3 offsetFromPlayer = playerCamera.transform.forward * swordAwayOffset;
			offsetFromPlayer.y = 0;
			// Rotate the offset based on the rotational velocity
			yRotation += yRotationalVelocity;
			print (yRotation);
			Quaternion rotationalVelocityRotation = Quaternion.Euler(0.0f, yRotation, 0.0f);
			offsetFromPlayer = rotationalVelocityRotation * offsetFromPlayer;

			swordObject.transform.position = new Vector3(0.0f, swordYOffset, 0.0f) + playerCamera.transform.position + offsetFromPlayer;

			// modify the rotation of the sword
			swordObject.transform.rotation = Quaternion.Euler (swordObject.transform.rotation.eulerAngles.x, 
               playerCamera.transform.rotation.eulerAngles.y + 180 + yRotation, swordObject.transform.rotation.eulerAngles.z);
		}
	}

	IEnumerator arcCooldown()
	{
		yield return new WaitForSeconds (swordActiveDuration);
		swordObject.SetActive (false);
		attacking = false;
		swordAwayOffset = oldSwordAwayOffset;
		yRotation = 0;
	}
}


