using UnityEngine;
using System.Collections;

public class MoveBolt : MonoBehaviour 
{
	public GameLogic game;
	public GameObject explosion;
	public int damage;
	public bool shouldMoveEnemies;
	public float damageInterval;
	public int damageAmount;
	public bool explodeOnContact;
	private CapsuleCollider damageCapsule;
	private bool dealingAreaDamage = false;
	private Vector3 velocity;
	private GameObject target;
	private int hashValue;


	// Use this for initialization
	void Start () 
	{
		hashValue = 0; // Default hash value
		damageCapsule = gameObject.GetComponent<CapsuleCollider> ();
		if (damageCapsule != null)
		{
			startPeriodicDamage();
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (velocity != null)
		{
			if (target != null)
			{
				Vector3 toTarget = target.transform.position - transform.position;
				toTarget.Normalize();
				toTarget *= .2f;
				toTarget.y = 0;
				velocity = toTarget;
			}
			//gameObject.transform.position += velocity;
		}
		
	}

	public void startPeriodicDamage()
	{
		StartCoroutine(periodicDamageInCapsule());
	}

	public void stopPeriodicDamage()
	{
		StopCoroutine (periodicDamageInCapsule ());
	}

	IEnumerator periodicDamageInCapsule()
	{
		while (true)
		{
			yield return new WaitForSeconds(damageInterval);
			checkToDealDamageInCapsule();
		}
	}

	public void checkToDealDamageInCapsule()
	{
		float radius = damageCapsule.radius;

		Vector3 point1 = transform.GetChild(0).position;
		Vector3 point2 = transform.GetChild (1).position;

		float distance = damageCapsule.height;
		Vector3 capsuleDirection = transform.rotation * new Vector3 (0.0f, 0.0f, 1.0f);

		RaycastHit[] hits = Physics.CapsuleCastAll(point1, point2, radius, capsuleDirection);
		for (int i = 0; i < hits.Length; i++)
		{
			// Check for enemies in hit
			BasicEnemyController enemy = (BasicEnemyController) hits[i].collider.gameObject.GetComponent(typeof(BasicEnemyController));
			if (enemy != null)
			{
				enemy.dealDamage(damageAmount);
			}
		}
	
	}

	public void setVelocity(Vector3 newVelocity)
	{
		velocity = newVelocity;
	}

	public void setHash(int newHash)
	{
		hashValue = newHash;
	}

	public int getHash()
	{
		return hashValue;
	}

//	private ParticleSystem.CollisionEvent[] collisionEvents = new ParticleSystem.CollisionEvent[16];

	void OnParticleCollision(GameObject other) 
	{
		print ("other name: " + other.name);

		if (other.name == "OilSlickCollider")
		{
			OilSlick oil = (OilSlick) other.transform.parent.gameObject.GetComponent(typeof(OilSlick));
			oil.blowUp();
			Destroy (gameObject);
		}


		
		BasicEnemyController enemy = (BasicEnemyController) other.GetComponent(typeof(BasicEnemyController));	
		TutorialEnemyController tutorialEnemy = (TutorialEnemyController)other.GetComponent (typeof(TutorialEnemyController));
		MoveBolt moveScript = (MoveBolt) other.GetComponent ((typeof(MoveBolt)));
		// Ignore collisions with other bolts
		if (moveScript != null)
		{

		}
		// Ignore collisions with player's hands
		else if (other.name == "palm" || other.name == "bone1" || other.name == "bone2")
		{

		}
		// Check if a player or their leap motion hands are hit
		else if (other.name == "LeapOVRPlayerController")
		{
//			if (!game.isPlayerBlocking())
//			{
//				Instantiate (explosion, transform.position, Quaternion.identity);
//				PlayerLogic hitPlayer = (PlayerLogic) other.GetComponent(typeof(PlayerLogic));
//				if (!hitPlayer.isDefensivePlayer)
//					hitPlayer.respawn();
//			}
//			// Reflect back blocked bolts
//			else
//			{
//				reverseVelocity();
//				game.reverseProjectileOnOtherClients(hashValue);
//			}
		}
		// Collide with an enemy
		else if (enemy != null)
		{
			if (explodeOnContact)
			{
				Instantiate (explosion, other.transform.position, Quaternion.identity);
			}

			if (shouldMoveEnemies)
				enemy.applyForce(velocity * 20 + new Vector3(0.0f, 10.0f, 0.0f));

			enemy.dealDamage(damage);
			// Find out where the collision point was 
			// Resize collision array if you have to
//			int safeLength = particleSystem.safeCollisionEventSize;
//			if (collisionEvents.Length < safeLength)
//				collisionEvents = new ParticleSystem.CollisionEvent[safeLength];
//			
//			particleSystem.GetCollisionEvents(other, collisionEvents);


				
//
//			Instantiate (explosion, collisionEvents[0].intersection, Quaternion.LookRotation (collisionEvents[0].normal.normalized));
//			Destroy(gameObject);
		}
		else if (tutorialEnemy != null)
		{
			if (shouldMoveEnemies)
				tutorialEnemy.applyForce(velocity * 20 + new Vector3(0.0f, 10.0f, 0.0f));
			
			tutorialEnemy.dealDamage(damage);
		}
		else
		{
//			Instantiate (explosion, transform.position, Quaternion.identity);
//			Destroy(gameObject);
		}
	}

	public void reverseVelocity()
	{
		velocity *= -1.0f;
	}

	public void setTarget(GameObject newTarget)
	{
		target = newTarget;
	}
}
