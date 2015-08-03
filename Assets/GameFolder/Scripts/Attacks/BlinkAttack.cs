using UnityEngine;
using System.Collections;


public class BlinkAttack : AAttack
{
	public GameObject playerCamera;
	public PlayerLogic player;
	public float blinkDistance;

    //public override void chargingFunction(HandModel[] hands)
    //{
    //    //		MoveFireball fireball = (MoveFireball) flameThrowerParticle.GetComponent (typeof(MoveFireball));
    //    //		fireball.stopPeriodicDamage();
    //    //		flameThrowerParticle.SetActive (false);
    //}
	
    //public override void chargedFunction(HandModel[] hands)
    //{
    //    // DEACTIVATE ABILITY
    //}
	
    //public override void releaseFunction(HandModel[] hands)
    //{
    //    Vector3 newCameraForward = new Vector3 (playerCamera.transform.forward.x, 0.0f, playerCamera.transform.forward.z);
    //    print ("Distance moving: " + (newCameraForward * blinkDistance));
    //    print ("Position before: " + playerCamera.transform.position);
    //    player.transform.position = new Vector3(player.transform.position.x + (newCameraForward * blinkDistance).x, player.transform.position.y, player.transform.position.z + (newCameraForward * blinkDistance).z);
    //    print ("Position after: " + playerCamera.transform.position);
    //}
	
    //public override void holdGestureFunction(HandModel[] hands)
    //{

    //}
	
    //public override void inactiveFunction()
    //{
    //    //		if (flamethrowers [0] == null || flamethrowers[1] == null)
    //    //			return;
    //    //		MoveFireball fireball1 = (MoveFireball) flamethrowers[0].GetComponent (typeof(MoveFireball));
    //    //		MoveFireball fireball2 = (MoveFireball) flamethrowers[1].GetComponent (typeof(MoveFireball));
    //    //		fireball1.stopPeriodicDamage();
    //    //		fireball2.stopPeriodicDamage();
    //    //		
    //    //		//		MoveFireball fireball = (MoveFireball) flameThrowerParticle.GetComponent (typeof(MoveFireball));
    //    //		//		fireball.stopPeriodicDamage();
    //    //		flamethrowers[0].SetActive (false);
    //    //		flamethrowers[1].SetActive (false);
    //}





}


