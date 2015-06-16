using UnityEngine;
using System.Collections;

public class SwordAttack : AAttack
{
	public GameObject swordObject;
	public float swordYOffset;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
    //public override void chargingFunction(HandModel[] hands)
    //{
    //    //		MoveFireball fireball = (MoveFireball) flameThrowerParticle.GetComponent (typeof(MoveFireball));
    //    //		fireball.stopPeriodicDamage();
    //    //		flameThrowerParticle.SetActive (false);
    //}
	
    //public override void chargedFunction(HandModel[] hands)
    //{
    //    swordObject.SetActive (true);
    //}
	
    //public override void releaseFunction(HandModel[] hands)
    //{
    //    swordObject.transform.position = hands [0].GetPalmPosition();
    //    swordObject.transform.rotation = hands [0].GetPalmRotation();
    //    swordObject.transform.RotateAround (swordObject.transform.position, new Vector3(0.0f, 0.0f, 1.0f), 270);
    //    swordObject.transform.position += swordObject.transform.rotation * new Vector3 (0.0f, swordYOffset, 0.0f);
    //}
	
    //public override void holdGestureFunction(HandModel[] hands)
    //{
    //    swordObject.transform.position = hands [0].GetPalmPosition();
    //    swordObject.transform.rotation = hands [0].GetPalmRotation();
    //    swordObject.transform.RotateAround (swordObject.transform.position, new Vector3(0.0f, 0.0f, 1.0f), 270);
    //    swordObject.transform.position += swordObject.transform.rotation * new Vector3 (0.0f, swordYOffset, 0.0f);
    //}
	
    //public override void inactiveFunction()
    //{
    //    swordObject.SetActive (false);
    //}
}
