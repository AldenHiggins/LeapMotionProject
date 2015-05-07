using UnityEngine;
using System.Collections;

public class GunAttack : AAttack
{
	public GameObject playerCamera;
	public int damageAmount;
	public AudioClip shotSound;
	private AudioSource audioSource;
	public GameObject gunShotLine;

	void Start()
	{
		audioSource = gameObject.GetComponent<AudioSource> ();
	}

	public override void chargingFunction(HandModel[] hands)
	{
		
	}
	
	public override void chargedFunction(HandModel[] hands)
	{}

	// Always use the first hand in the array, should change later
	public override void releaseFunction(HandModel[] hands)
	{


		RaycastHit hitPoint = new RaycastHit();
		// Only hit enemies, which are in the eigth physics layer
		int layerMask = 1 << 8;
		Ray ray = new Ray(playerCamera.gameObject.transform.position, playerCamera.gameObject.transform.forward);
		if (Physics.Raycast(ray, out hitPoint, 30.0f, layerMask))
		{
			BasicEnemyController enemy = (BasicEnemyController) hitPoint.collider.gameObject.GetComponent(typeof(BasicEnemyController));
			if (enemy != null)
			{
				// only play the gunshot sound if you hit an enemy and dealt damage
				if (audioSource != null)
				{
					audioSource.PlayOneShot(shotSound);
				}
				enemy.dealDamage(damageAmount);
				GameObject newGunShotLine = (GameObject) Instantiate(gunShotLine);
				newGunShotLine.SetActive(true);
				LineRenderer line = newGunShotLine.GetComponent<LineRenderer>();
				line.SetPosition(0, hands[0].GetPalmPosition());
				line.SetPosition(1, hitPoint.point);
			}
		}


		
	}
	
	public override void holdGestureFunction(HandModel[] hands)
	{
		
	}
	
	public override void inactiveFunction()
	{
		
	}


}


