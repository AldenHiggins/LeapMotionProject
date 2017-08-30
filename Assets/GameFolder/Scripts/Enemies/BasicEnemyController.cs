﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BasicEnemyController : MonoBehaviour, IUnit
{
	// MONSTER CHARACTERISTICS
	public float speed;
	public int startingHealth;
	public float attackRadius;
	public int attackDamage;
	public int currencyOnKill;
	public int livesTakenOnGoalReached;
	public bool isAlly;
	// HEADSHOTS
	public float headshotHeight;
	// ANIMATOR
	private Animator anim;
	private int health;
	private bool attacking;
	// MOVEMENT
	private UnityEngine.AI.NavMeshAgent agent;
	private GameObject target;
	private Vector3 velocity;
	public bool usesRootMotion;
	public bool floatingEnemy;
	public float floatingVelocity;
	private GameObject goalPosition;
	// AUDIO
	private AudioSource source;
	public AudioClip woundSound;
	public AudioClip killSound;
	// RAGDOLL/DEATH
	private bool isDying;

	// Use this for initialization
	void Start () 
	{
		attacking = false;
		isDying = false;

        goalPosition = GetObjects.getGoalPosition();

		anim = transform.GetChild (0).gameObject.GetComponent<Animator> ();
		
		health = startingHealth;

        //<<<<<<< HEAD

        agent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        //=======
        //agent = gameObject.GetComponent<NavMeshAgent> ();
        //>>>>>>> c5c8868af13028fa52019e2b91ab548f24e18634
        agent.updateRotation = true;

		if (floatingEnemy)
		{
			agent.enabled = false;
		}
			
		source = GetComponent<AudioSource> ();
	}

	// Update is called once per frame
	void Update () 
	{
		if (health <= 0)
		{
			return;
		}
	
		// Search for nearby enemies to attack
		GameObject enemy = null;
		
		// Only gets allies and player
		int enemySearchLayer = 1 << 16;
		enemySearchLayer = enemySearchLayer | (1 << 11);

		if (isAlly)
		{
			enemySearchLayer = 1 << 8; 
		}

		RaycastHit[] rayCastHits = Physics.SphereCastAll(transform.position, 3.0f, transform.forward, 10.0f, enemySearchLayer);
        float minimumDistance = float.MaxValue;
		// Just find the closest enemy
		for (int hitIndex = 0; hitIndex < rayCastHits.Length; hitIndex++)
		{
			IUnit enemyUnit = (IUnit) rayCastHits[hitIndex].collider.gameObject.GetComponent(typeof(IUnit));
			if (enemyUnit != null)
			{
				GameObject enemyFound = enemyUnit.getGameObject();
                float enemyDistance = Vector3.Distance(enemyFound.transform.position, gameObject.transform.position);
                if (enemyDistance < minimumDistance)
                {
                    enemy = enemyFound;
                    minimumDistance = enemyDistance;
                }
			}
		}

		if (enemy != null)
		{
			target = enemy;
		}
        else
        {
            if (isAlly)
            {
                return;
            }

            target = goalPosition;
        }

        velocity = target.transform.position - transform.position;
		velocity.y = 0.0f;
		// If the enemy is outside melee range keep coming forward
		if (velocity.magnitude > attackRadius)
		{
			if (attacking == false)
			{
				anim.SetBool ("Running", true);

				// If the enemy doesn't float use the nav mesh
				if (!floatingEnemy)
				{
					if (agent.isActiveAndEnabled)
					{
						agent.Resume();
						agent.SetDestination (target.transform.position);
					} 
					else
					{
						agent.enabled = true;
						agent.Resume();
						agent.SetDestination (target.transform.position);
					}
				}
				// If it does just move on forward towards the player
				else
				{
					Vector3 targetVector = target.transform.position - gameObject.transform.position;
					gameObject.transform.position += Time.deltaTime * targetVector * floatingVelocity;
				}
			}
		}
		// If the player is in range attack him
		else
		{
			anim.SetBool ("Running", false);

			if (!floatingEnemy)
			{
				agent.Stop ();
			}

            //Face the target as well
            transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);
            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);

			// Don't start attacking if no enemy was found
			if (enemy != null)
			{
				if (attacking == false)
				{
					attacking = true;
					StartCoroutine(attack());
				}
			}
		}

        if (floatingEnemy)
        {
            //Face the target as well
		    transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);
		    transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        }
	}

	IEnumerator attack()
	{
		anim.SetBool ("Attacking", true);
		// All enemies globally do damage after a second and a half
		yield return new WaitForSeconds (.5f);

		Vector3 distance = target.transform.position - transform.position;
		distance.y = 0.0f;
		if (distance.magnitude < attackRadius && !isDying)
		{
			IUnit unitToAttack = (IUnit) target.GetComponent(typeof(IUnit));
			if (unitToAttack != null)
			{
				unitToAttack.dealDamage(attackDamage);
			}
		}

        yield return new WaitForSeconds(1.0f);

		attacking = false;
		anim.SetBool ("Attacking", false);
	}

	public void dealDamage(int damage)
	{
		health -= damage;

		if (health > 0)
		{
            anim.Play ("wound");
			source.PlayOneShot(woundSound);
		}
		else
		{
			source.PlayOneShot(killSound);

            kill();
		}
	}

	public void applyForce(Vector3 force)
	{
		GetComponent<Rigidbody>().AddForce (force, ForceMode.Impulse);
//		agent.enabled = false;
		GetComponent<Rigidbody>().isKinematic = false;
		StartCoroutine (restartAgent ());
	}

	public void applyExplosiveForce(float force, Vector3 position, float radius)
	{
		GetComponent<Rigidbody>().AddExplosionForce (force, position, radius, 20.0f, ForceMode.Impulse);
//		agent.enabled = false;
		GetComponent<Rigidbody>().isKinematic = false;
		StartCoroutine (restartAgent ());
	}

	IEnumerator restartAgent ()
	{
		while (GetComponent<Rigidbody>().velocity.magnitude > 0.1)
		{
			yield return new WaitForSeconds(.1f);
		}
		agent.enabled = true;
	}

    void kill()
    {
        isDying = true;
        if (agent.enabled)
        {
            agent.enabled = false;
        }
        BoxCollider collider = gameObject.GetComponent<BoxCollider>();
        collider.enabled = false;
        StopCoroutine(attack());
        anim.SetBool("Attacking", false);
        anim.SetBool("Dead", true);
        StartCoroutine(waitToDestroy());
    }

//<<<<<<< HEAD
	private void displayNavMeshPath()
	{
		// Show nav mesh paths
		if (agent.hasPath)
		{
			UnityEngine.AI.NavMeshPath thisPath = agent.path;
			Vector3[] pathVertices = thisPath.corners;
			
			LineRenderer lineRender;
			
			// Draw a line to show the player where they are aiming
			//lineRender = (LineRenderer) thisPathLine.GetComponent<Renderer>();
			//lineRender.enabled = true;
			
			//lineRender.SetColors (Color.yellow, Color.yellow);
			//lineRender.SetVertexCount (pathVertices.Length);
			////			print ("Path length: " + pathVertices.Length);
			//for (int i = 0; i < pathVertices.Length; i++)
			//{
			//	lineRender.SetPosition (i, pathVertices[i]);
			//}
		}
//=======
        // Start the coroutine that will wait for the monster to be destroyed
        //StartCoroutine(waitToDestroy());
    }
//>>>>>>> c5c8868af13028fa52019e2b91ab548f24e18634

	IEnumerator waitToDestroy()
	{
		yield return new WaitForSeconds (10.0f);
        Destroy (gameObject);
	}

	public void slowDown()
	{
		// Don't slow down floating enemies
		if (floatingEnemy)
		{
			return;
		}

        agent.speed = 1;

		//StopCoroutine(slowNavMesh());
		//StartCoroutine(slowNavMesh());
	}

	IEnumerator slowNavMesh()
	{
		agent.speed = 1;
		yield return new WaitForSeconds (3.0f);
		agent.speed = 2;
	}

	public bool isMonsterDying()
	{
		return isDying;
	}

	public GameObject getGameObject()
	{
		return gameObject;
	}
}
