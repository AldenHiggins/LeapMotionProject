using UnityEngine;
using System.Collections;

public class MoveFireball : MonoBehaviour 
{
	private PlayerLogic player;

	public GameLogic game;

	public GameObject explosion;

	public int damage;

	public bool shouldMoveEnemies;

	public float damageInterval;

	public int damageAmount;

	private CapsuleCollider damageCapsule;

	private bool dealingAreaDamage = false;

	private Vector3 velocity;

	private GameObject target;

	private int hashValue;

	public bool explodeOnContact;

	public int specialGainPerHeadshot;

	// SAVE TO CHARGE FLAMETHROWERS WITH HEADSHOTS ONLY
	public OffensiveAbilities offense;

	// Use this for initialization
	void Start () 
	{
        player = GetObjects.getPlayer();
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
//		print ("Checking to deal flamethrower damage");
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
        if ((other.name == "OilSlickCollider")&&(gameObject.name == "CrazyFireball(Clone)"))
		{
			OilSlick oil = (OilSlick) other.transform.parent.gameObject.GetComponent(typeof(OilSlick));
			oil.blowUp();
			Destroy (gameObject);
		}	
		
		BasicEnemyController enemy = (BasicEnemyController) other.GetComponent(typeof(BasicEnemyController));
		if (enemy == null) enemy = (BasicEnemyController) other.GetComponentInChildren(typeof(BasicEnemyController));	
		TutorialEnemyController tutorialEnemy = (TutorialEnemyController)other.GetComponent (typeof(TutorialEnemyController));
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
			ParticleSystem currentParticle = (ParticleSystem) gameObject.GetComponent(typeof(ParticleSystem));
			ParticleCollisionEvent[] collisions = new ParticleCollisionEvent[16];
			int nummberCollisions = currentParticle.GetCollisionEvents(other, collisions);

			if (shouldMoveEnemies)
				enemy.applyForce(velocity * 20 + new Vector3(0.0f, 10.0f, 0.0f));


			// Check for headshots with zombies
			if (collisions[0].intersection.y > enemy.headshotHeight)
			{
				if (explodeOnContact)
				{
					GameObject createdExplosion = (GameObject) Instantiate (explosion, collisions[0].intersection, Quaternion.identity);
					createdExplosion.SetActive(true);
					player.addSpecialAttackPower(specialGainPerHeadshot);
				}
			}

			enemy.dealDamage(damage);
			Destroy(gameObject);
		}
		else if (tutorialEnemy != null)
		{
			if (shouldMoveEnemies)
				tutorialEnemy.applyForce(velocity * 20 + new Vector3(0.0f, 10.0f, 0.0f));
			
			tutorialEnemy.dealDamage(damage);
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
