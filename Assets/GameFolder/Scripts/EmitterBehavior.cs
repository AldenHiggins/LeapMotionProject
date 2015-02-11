using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EmitterBehavior : MonoBehaviour 
{
	public GameObject fireBall;
	

	private int fireballTimer;
	private int blockTimer;

	// Use this for initialization
	void Start () 
	{
		fireballTimer = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{	
//		// Only make the fireball if you're the server for synchronization purposes (so the two fireballs generated at the same time
//		// won't have different hashes)
//		if (network.isServer)
//		{
		// Acquire the nearest target
		Collider[] nearbyObjects = Physics.OverlapSphere (transform.position, 10);
		float minDistance = float.MaxValue;
		BasicEnemyController nearestEnemy = null;
		for (int i = 0; i < nearbyObjects.Length; i++)
		{
			if (nearbyObjects[i].transform.childCount > 0)
			{
				BasicEnemyController enemy = (BasicEnemyController) nearbyObjects[i].gameObject.GetComponent(typeof(BasicEnemyController));
				if (enemy != null)
				{
					float distance = Vector3.Distance (transform.position, enemy.transform.position);
					if (distance < minDistance)
					{
						minDistance = distance;
						nearestEnemy = enemy;
					}
				}
			}
		}

		if (nearestEnemy != null)
		{
			// Turn to face the enemy
			transform.rotation = Quaternion.LookRotation(nearestEnemy.transform.position - transform.position);
			transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
		}

		// Temp fireball launcher to test
		fireballTimer++;
		if (fireballTimer > 100)
		{
			fireballTimer = 0;
			Vector3 velocity = transform.forward.normalized;
			Vector3 startPosition = transform.position + velocity * 1;
			velocity *= .1f;
			createFireball(startPosition, transform.rotation, velocity, 0);
//			view.RPC ("makeFireballNetwork", RPCMode.Others, new Vector3(-4.6f, 76.75f, 1.8f), Quaternion.identity, new Vector3(0.0f, 0.0f, -0.1f), hash);
		}
	}
	
//	[RPC]
//	public void makeFireballNetwork(Vector3 position, Quaternion rotation, Vector3 velocity, int hashValue)
//	{
//		createFireball (position, rotation, velocity, hashValue);
//	}
	
	public void createFireball(Vector3 position, Quaternion rotation, Vector3 velocity, int hashValue)
	{
		GameObject newFireball = (GameObject) Instantiate(fireBall, position, rotation);
		MoveFireball moveThis = (MoveFireball) newFireball.GetComponent(typeof(MoveFireball));
		moveThis.setVelocity(velocity);
		newFireball.renderer.enabled = true;
		moveThis.setHash (hashValue);
		// Also enable this particle's self destruct feature
		ProjectileDestroy destroyThis = (ProjectileDestroy) newFireball.GetComponent (typeof(ProjectileDestroy));
		destroyThis.enabled = true;
	}
}
