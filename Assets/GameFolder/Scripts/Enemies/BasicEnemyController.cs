using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BasicEnemyController : MonoBehaviour, IUnit
{
	// GAME LOGIC
	public GameLogic game;
	// PLAYER
	PlayerLogic player;
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
	// NAV MESH AND PATH DEBUGGING
	public bool showNavMeshPath;
	public GameObject pathLine;
	private GameObject thisPathLine;
	// ANIMATOR
	private Animator anim;
	private int health;
	private bool attacking;
	// MOVEMENT
	private NavMeshAgent agent;
	private GameObject target;
	private Vector3 velocity;
	public int waitToMove;
	private int startingMoveCounter;
	public bool usesRootMotion;
	public bool floatingEnemy;
	public float floatingVelocity;
	public GameObject goalPosition;
	// AUDIO
	private AudioSource source;
	public AudioClip woundSound;
	public AudioClip killSound;
	// HEALTH BARS
	public bool showHealthBar;
	public GameObject greenHealthBar;
	public GameObject redHealthBar;
	public float healthBarVerticalOffset;
	private GameObject greenHealth;
	private GameObject redHealth;
	private float startingHealthScale;
	public GameObject damageAmountUI;
	// RAGDOLL/DEATH
	public float ragdollTime;
	public float ragdollForceFactor;
	public GameObject ragDollCenterObject;
	public bool usesRagdoll;
	private bool isDying;
	// TUTORIAL
	public bool isTutorial;
	public GameObject tutorialGoalTarget;


	// Use this for initialization
	void Start () 
	{
		attacking = false;
		isDying = false;

        player = GetObjects.getPlayer();

		if (usesRootMotion)
		{
			anim = gameObject.GetComponent<Animator> ();
		}
		else
		{
			anim = transform.GetChild (0).gameObject.GetComponent<Animator> ();
		}

		health = startingHealth;


		agent = gameObject.GetComponent<NavMeshAgent> ();
		agent.updateRotation = true;

		if (floatingEnemy)
		{
			agent.enabled = false;
		}

		if (isTutorial)
		{
			target = tutorialGoalTarget;
		}
		else
		{
			target = player.gameObject;
		}

		if (showNavMeshPath)
		{
			thisPathLine = (GameObject) Instantiate (pathLine);
		}
			
		source = GetComponent<AudioSource> ();
		startingMoveCounter = 0;

		if (showHealthBar)
		{
			greenHealth = (GameObject) Instantiate(greenHealthBar);
			redHealth = (GameObject) Instantiate(redHealthBar);
			greenHealth.transform.parent = gameObject.transform;
			redHealth.transform.parent = gameObject.transform;
			greenHealth.transform.localPosition = new Vector3(0.0f, healthBarVerticalOffset, 0.0f);
			redHealth.transform.localPosition = new Vector3(0.0f, healthBarVerticalOffset, 0.0f);
			startingHealthScale = greenHealth.transform.localScale.y;
		}
	}



	// Update is called once per frame
	void Update () 
	{
		if (health <= 0)
		{
			return;
		}
	
		startingMoveCounter++;

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

		if (showNavMeshPath)
		{
			displayNavMeshPath ();
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
		if (damageAmountUI != null)
		{
			GameObject thisDamage = (GameObject) Instantiate(damageAmountUI, (transform.position + new Vector3(0.0f, headshotHeight, 0.0f)), Quaternion.identity);
			thisDamage.SetActive(true);

			// Get the text field of the damage popup
			Text textFieldAmountOfDamage = thisDamage.transform.GetChild (1).GetChild(0).GetComponent<Text>();
			textFieldAmountOfDamage.text = "" + damage;
		}
		// TEMP TEMP TEMP check remove after forces can affect enemies
		if (anim == null)
			return;
		health -= damage;

		if (health > 0)
		{
			greenHealth.transform.localScale = new Vector3(greenHealth.transform.localScale.x, (((float)health / startingHealth) * startingHealthScale), greenHealth.transform.localScale.z);
			anim.Play ("wound");
			source.PlayOneShot(woundSound);
		}
		else
		{
			Destroy (greenHealth);
			Destroy (redHealth);
			source.PlayOneShot(killSound);

			StartCoroutine(kill ());
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
//		rigidbody.isKinematic = true;
	}

	IEnumerator kill()
	{
		isDying = true;
		if (agent.enabled)
		{
			agent.enabled = false;
		}
		BoxCollider collider = gameObject.GetComponent<BoxCollider> ();
		collider.enabled = false;
		StopCoroutine (attack ());
		player.changeCurrency (currencyOnKill);
		anim.SetBool ("Attacking", false);
		anim.SetBool ("Dead", true);
//		while(!anim.GetCurrentAnimatorStateInfo(0).IsName("death"))
//		{
//			yield return new WaitForSeconds(.1f);
//		}
		yield return new WaitForSeconds (3.0f);
		if (usesRagdoll)
		{
			Destroy (this.transform.parent.gameObject);
		}
		Destroy (this.gameObject);
	}

	IEnumerator ragdollKill()
	{
		isDying = true;
//		agent.enabled = false;
		anim.enabled = false;
		BoxCollider collider = gameObject.GetComponent<BoxCollider> ();
		collider.enabled = false;
		Rigidbody ragDollRigidBody = ragDollCenterObject.GetComponent<Rigidbody> ();
		ragDollRigidBody.AddForce (new Vector3 (0, 5 * ragdollForceFactor, -10 * ragdollForceFactor), ForceMode.Impulse);
		yield return new WaitForSeconds (ragdollTime);
		Destroy (transform.parent.gameObject);
	}

	private void displayNavMeshPath()
	{
		// Show nav mesh paths
		if (agent.hasPath)
		{
			NavMeshPath thisPath = agent.path;
			Vector3[] pathVertices = thisPath.corners;
			
			LineRenderer lineRender;
			
			// Draw a line to show the player where they are aiming
			lineRender = (LineRenderer) thisPathLine.GetComponent<Renderer>();
			lineRender.enabled = true;
			
			lineRender.SetColors (Color.yellow, Color.yellow);
			lineRender.SetVertexCount (pathVertices.Length);
			//			print ("Path length: " + pathVertices.Length);
			for (int i = 0; i < pathVertices.Length; i++)
			{
				lineRender.SetPosition (i, pathVertices[i]);
			}
		}
	}

	// Sets the navmeshAgent to use the animation's velocity (root motion)
	void OnAnimatorMove()
	{
		if (agent != null)
			agent.velocity = anim.deltaPosition / Time.deltaTime;
	}

	public void slowDown()
	{
		// Don't slow down floating enemies
		if (floatingEnemy)
		{
			return;
		}

		if (usesRootMotion)
		{
			StopCoroutine(slowRootMotion());
			StartCoroutine(slowRootMotion());
		}
		else
		{
			StopCoroutine(slowNavMesh());
			StartCoroutine(slowNavMesh());
		}
	}

	IEnumerator slowRootMotion()
	{
		anim.SetBool ("Slowed", true);
		yield return new WaitForSeconds (3.0f);
		anim.SetBool ("Slowed", false);
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
