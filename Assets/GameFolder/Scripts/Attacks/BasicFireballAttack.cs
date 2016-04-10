using UnityEngine;
using System.Collections;

public class BasicFireballAttack : AAttack 
{
	public PlayerLogic player;
	public GameLogic game;
	public GameObject thisCamera;
	public GameObject fireBall;
	public GameObject projectileActiveParticle;
	public int manaCost;


	public float firingCoolDown;

	private bool canFire;

	
	// Use this for initialization
	void Start () 
	{
		canFire = true;
	}

	
    public void chargingFunction(RigidHand hands)
    {

    }
	
	public void chargedFunction(RigidHand hands)
    {}
	
	public void releaseFunction(RigidHand hands)
    {
        // Check and see if the cooldown is up
        if (canFire != true)
        {
            return;
        }

        // Wait for the next cool down in order to fire again
        if (projectileActiveParticle != null)
        {
            projectileActiveParticle.SetActive (false);
        }

        canFire = false;
        StartCoroutine (waitForCoolDown ());

        if (player.getEnergy() < manaCost)
        {
            return;
        }
		
        if (manaCost != 0)
        {
            player.useEnergy (manaCost);
        }

        // Have the player spend mana
        // playerLogic.useEnergy(10);
        Vector3 spawnPosition = hands.GetPalmPosition();
        // Scale the fireball's velocity
        //Vector3 startingVelocity = hands.GetPalmNormal();
        //print("Palm normal: " + startingVelocity[0] + " " + startingVelocity[1] + " " + startingVelocity[2]);
        Vector3 cameraNormal = thisCamera.transform.forward.normalized;
        print("Camera normal: " + cameraNormal[0] + " " + cameraNormal[1] + " " + cameraNormal[2]);

        //startingVelocity *= .2f;

        GameObject newFireball = (GameObject) Instantiate(fireBall, spawnPosition, Quaternion.LookRotation(cameraNormal));
        newFireball.SetActive(true); 
        MoveFireball moveThis = (MoveFireball) newFireball.GetComponent(typeof(MoveFireball));
        moveThis.setVelocity(cameraNormal);
        newFireball.GetComponent<Renderer>().enabled = true;
        moveThis.setHash (0);

    }
	
	public void holdGestureFunction(RigidHand hands)
    {
		
    }
	
    public void inactiveFunction()
    {

    }

	IEnumerator waitForCoolDown()
	{

		yield return new WaitForSeconds (firingCoolDown);
		if (projectileActiveParticle != null)
		{
			projectileActiveParticle.SetActive (true);
		}
		canFire = true;
	}
}
