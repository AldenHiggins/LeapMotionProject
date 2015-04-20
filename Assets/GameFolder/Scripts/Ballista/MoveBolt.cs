using UnityEngine;
using System.Collections;

public class MoveBolt : MonoBehaviour 
{
	public GameLogic game;
	public int damage;
	private Vector3 velocity;
	private GameObject target;
	private int hashValue;


	// Use this for initialization
	void Start () 
	{
		hashValue = 0; // Default hash value
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
			gameObject.transform.position += velocity;
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

	void OnCollisionEnter(Collision collision) 
	{
		//print ("Ballista collision!");
		if (collision.gameObject == null) 
		{
			//print ("No game object!");
			return;
		}

		GameObject other = collision.gameObject;

		//print ("other name: " + other.name);

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
			//print ("Colliding with another bolt");
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
			print ("Got here, should deal damage to enemies!");
			enemy.dealDamage(damage);
			// Find out where the collision point was 
			// Resize collision array if you have to
//			int safeLength = particleSystem.safeCollisionEventSize;
//			if (collisionEvents.Length < safeLength)
//				collisionEvents = new ParticleSystem.CollisionEvent[safeLength];
//			
//			particleSystem.GetCollisionEvents(other, collisionEvents);

			Destroy (gameObject);
				
//
//			Instantiate (explosion, collisionEvents[0].intersection, Quaternion.LookRotation (collisionEvents[0].normal.normalized));
//			Destroy(gameObject);
		}
		else if (tutorialEnemy != null)
		{
			tutorialEnemy.dealDamage(damage);
		}
		else
		{
//			Instantiate (explosion, transform.position, Quaternion.identity);
			Destroy(gameObject);
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
