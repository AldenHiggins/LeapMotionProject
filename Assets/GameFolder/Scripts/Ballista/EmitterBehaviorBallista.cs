using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EmitterBehaviorBallista : MonoBehaviour 
{
	public GameObject bolt;
	public float emissionVelocity;
	public float emissionFrequency;
	public float attackRadius;
	public bool multiAttack;

	private int boltTimer;
	private int blockTimer;

	// Use this for initialization
	void Start () 
	{
		boltTimer = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{	
//		// Only make the bolt if you're the server for synchronization purposes (so the two bolts generated at the same time
//		// won't have different hashes)
//		if (network.isServer)
//		{
		// Acquire the nearest target
		Collider[] nearbyObjects = Physics.OverlapSphere (transform.position, attackRadius);
		float minDistance = float.MaxValue;
		BasicEnemyController nearestEnemy = null;
		TutorialEnemyController nearestTutEnemy = null;
		for (int i = 0; i < nearbyObjects.Length; i++)
		{
			if (nearbyObjects[i].transform.childCount > 0)
			{
				BasicEnemyController enemy = (BasicEnemyController) nearbyObjects[i].gameObject.GetComponent(typeof(BasicEnemyController));
				TutorialEnemyController enemyTut = (TutorialEnemyController) nearbyObjects[i].gameObject.GetComponent(typeof(TutorialEnemyController));
				if (enemy != null)
				{
					float distance = Vector3.Distance (transform.position, enemy.transform.position);
					if (distance < minDistance)
					{
						minDistance = distance;
						nearestEnemy = enemy;
					}
				}

				if (enemyTut != null)
				{
					float distance = Vector3.Distance (transform.position, enemyTut.transform.position);
					if (distance < minDistance)
					{
						minDistance = distance;
						nearestTutEnemy = enemyTut;
					}
				}
			}
		}

		if (nearestEnemy != null)
		{
			// Turn to face the enemy
			transform.rotation = Quaternion.LookRotation(nearestEnemy.transform.position - transform.position);
			transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);

			// Launch bolt as long as an enemy is found
			boltTimer++;
			if (boltTimer > emissionFrequency)
			{
				boltTimer = 0;
				Vector3 velocity = transform.forward.normalized;
				Vector3 startPosition = transform.position + velocity * 1;
				// Apply the correct velocity to the emission
				velocity *= emissionVelocity;
				GameObject newbolt = createbolt(startPosition, transform.rotation, velocity, 0);
				MoveBolt bolt = (MoveBolt) newbolt.GetComponent(typeof(MoveBolt));
				bolt.setTarget(nearestEnemy.gameObject);

				// Fire three particles for a multi-attack
				if (multiAttack)
				{
					Quaternion newRotation = transform.rotation * Quaternion.Euler (15 * Vector3.up);
					Vector3 newVelocity =  newRotation * Vector3.forward;
					newVelocity.Normalize();
					newVelocity *= emissionVelocity;
					
					createbolt(startPosition, Quaternion.identity, newVelocity, 0);

					Quaternion rotation2 = transform.rotation * Quaternion.Euler (-15 * Vector3.up);
					Vector3 secondVelocity =  rotation2 * Vector3.forward;
					secondVelocity.Normalize();
					secondVelocity *= emissionVelocity;
					
					createbolt(startPosition, Quaternion.identity, secondVelocity, 0);
//					createbolt(startPosition, Quaternion.AngleAxis(yRotation + 15, Vector3.up), velocity, 0);
				}
				//view.RPC ("makeboltNetwork", RPCMode.Others, new Vector3(-4.6f, 76.75f, 1.8f), Quaternion.identity, new Vector3(0.0f, 0.0f, -0.1f), hash);
			}
		}
		// TODO: CHANGE THIS
		else if (nearestTutEnemy != null)
		{
			// Turn to face the enemy
			transform.rotation = Quaternion.LookRotation(nearestTutEnemy.transform.position - transform.position);
			transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
			
			// Launch bolt as long as an enemy is found
			boltTimer++;
			if (boltTimer > emissionFrequency)
			{
				boltTimer = 0;
				Vector3 velocity = transform.forward.normalized;
				Vector3 startPosition = transform.position + velocity * 1;
				// Apply the correct velocity to the emission
				velocity *= emissionVelocity;
				GameObject newbolt = createbolt(startPosition, transform.rotation, velocity, 0);
				MoveBolt bolt = (MoveBolt) newbolt.GetComponent(typeof(MoveBolt));
				bolt.setTarget(nearestTutEnemy.gameObject);
				
				// Fire three particles for a multi-attack
				if (multiAttack)
				{
					Quaternion newRotation = transform.rotation * Quaternion.Euler (15 * Vector3.up);
					Vector3 newVelocity =  newRotation * Vector3.forward;
					newVelocity.Normalize();
					newVelocity *= emissionVelocity;
					
					createbolt(startPosition, Quaternion.identity, newVelocity, 0);
					
					Quaternion rotation2 = transform.rotation * Quaternion.Euler (-15 * Vector3.up);
					Vector3 secondVelocity =  rotation2 * Vector3.forward;
					secondVelocity.Normalize();
					secondVelocity *= emissionVelocity;
					
					createbolt(startPosition, Quaternion.identity, secondVelocity, 0);
					//					createbolt(startPosition, Quaternion.AngleAxis(yRotation + 15, Vector3.up), velocity, 0);
				}
				//view.RPC ("makeboltNetwork", RPCMode.Others, new Vector3(-4.6f, 76.75f, 1.8f), Quaternion.identity, new Vector3(0.0f, 0.0f, -0.1f), hash);
			}
		}
		else
		{
			transform.rotation = Quaternion.identity;
		}
	}
	
//	[RPC]
//	public void makeboltNetwork(Vector3 position, Quaternion rotation, Vector3 velocity, int hashValue)
//	{
//		createbolt (position, rotation, velocity, hashValue);
//	}
	
	public GameObject createbolt(Vector3 position, Quaternion rotation, Vector3 velocity, int hashValue)
	{
		GameObject newbolt = (GameObject) Instantiate(bolt, position, rotation);

		newbolt.SetActive(true); 
		MoveBolt moveThis = (MoveBolt) newbolt.GetComponent(typeof(MoveBolt));
		moveThis.setVelocity(velocity);
		newbolt.GetComponent<Renderer>().enabled = true;
		moveThis.setHash (0);

		return newbolt;
//		Movebolt moveThis = (Movebolt) newbolt.GetComponent(typeof(Movebolt));
//		moveThis.setVelocity(velocity);
//		newbolt.GetComponent<Renderer>().enabled = true;
//		moveThis.setHash (hashValue);
//		// Also enable this particle's self destruct feature
//		ProjectileDestroy destroyThis = (ProjectileDestroy) newbolt.GetComponent (typeof(ProjectileDestroy));
//		destroyThis.enabled = true;
		//return newbolt;
	}

	public void highlight(bool highlightOrNot)
	{
		// If highlighted show attack radius sphere
		if (highlightOrNot)
		{
			gameObject.transform.GetChild (0).gameObject.GetComponent<Renderer>().enabled = true;
		}
		// Else hide attack radius sphere
		else
		{
			gameObject.transform.GetChild (0).gameObject.GetComponent<Renderer>().enabled = false;
		}
	}
}
