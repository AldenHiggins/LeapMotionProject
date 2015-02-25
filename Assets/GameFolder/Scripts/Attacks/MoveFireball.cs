using UnityEngine;
using System.Collections;

public class MoveFireball : MonoBehaviour {
	public GameLogic game;

	public GameObject explosion;

	public int damage;

	public bool shouldMoveEnemies;

	private Vector3 velocity;

	private GameObject target;

	private int hashValue;

	// Use this for initialization
	void Awake () 
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
			//gameObject.transform.position += velocity;
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

	void OnParticleCollision(GameObject other) 
	{

		BasicEnemyController enemy = (BasicEnemyController) other.GetComponent(typeof(BasicEnemyController));	
		MoveFireball moveScript = (MoveFireball) other.GetComponent ((typeof(MoveFireball)));
		// Ignore collisions with other fireballs
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
//			// Reflect back blocked fireballs
//			else
//			{
//				reverseVelocity();
//				game.reverseProjectileOnOtherClients(hashValue);
//			}
		}
		// Collide with an enemy
		else if (enemy != null)
		{
			if (shouldMoveEnemies)
				enemy.applyForce(velocity * 20 + new Vector3(0.0f, 10.0f, 0.0f));

			enemy.dealDamage(damage);
			Instantiate (explosion, transform.position, Quaternion.identity);
			Destroy(gameObject);
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
