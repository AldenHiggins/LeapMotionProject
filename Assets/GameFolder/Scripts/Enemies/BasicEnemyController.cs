using UnityEngine;
using System.Collections;

public class BasicEnemyController : MonoBehaviour 
{
	// GAME LOGIC
	public GameLogic game;
	// PLAYER
	public PlayerLogic player;
	// MONSTER CHARACTERISTICS
	public float speed;
	public int startingHealth;
	public float attackRadius;
	public int attackDamage;
	public int currencyOnKill;
	public int livesTakenOnGoalReached;
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
//		target = game.getEnemyTarget ();
		if (isTutorial) target = tutorialGoalTarget;
		else target = player.gameObject;
		if (showNavMeshPath)
			thisPathLine = (GameObject) Instantiate (pathLine);
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
			return;
	
		startingMoveCounter++;
//		print ("Current velocity: " + rigidbody.velocity);

		velocity = target.transform.position - transform.position;
		velocity.y = 0.0f;
		// If the enemy is outside melee range keep coming forward
		if (velocity.magnitude > attackRadius)
		{
			if (attacking == false)
			{
				anim.SetBool ("Running", true);
				velocity.Normalize ();
				velocity *= speed;
				// Get this objects position based on this gameobject's child (i.e. the gameobject of the zombie that's being animated)
				if (startingMoveCounter > waitToMove)
				{
//					print ("Adding this position: " + gameObject.transform.GetChild(0).localPosition);
//					gameObject.transform.position += gameObject.transform.GetChild(0).localPosition;
//					gameObject.transform.GetChild(0).localPosition = new Vector3(0,0,0);
//					gameObject.transform.GetChild(0).rotation = Quaternion.identity;
				}

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
		}
		// If the player is in range attack him
		else
		{
			anim.SetBool ("Running", false);
			if (attacking == false)
			{
				attacking = true;
//				if (!usesRootMotion)
//				{
//					agent.Stop();
//				}
				agent.Stop ();
				StartCoroutine(attack());
			}
		}

		// Face the target as well
//		transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);
//		transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
		

		if (showNavMeshPath)
			displayNavMeshPath ();
	}

	IEnumerator attack()
	{
		anim.SetBool ("Attacking", true);
		// All enemies globally do damage after a second and a half
		yield return new WaitForSeconds (1.5f);

		Vector3 distance = target.transform.position - transform.position;
		distance.y = 0.0f;
		if (distance.magnitude < attackRadius && !isDying)
		{
			PlayerLogic playerToAttack = (PlayerLogic) target.GetComponent(typeof(PlayerLogic));
			if (playerToAttack != null)
				playerToAttack.dealDamage(attackDamage);
		}
		attacking = false;
		anim.SetBool ("Attacking", false);
	}



	public void dealDamage(int damage)
	{
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
		StopCoroutine (attack ());
		player.changeCurrency (currencyOnKill);
		anim.Play ("death");
		while(!anim.GetCurrentAnimatorStateInfo(0).IsName("death"))
		{
			yield return new WaitForSeconds(.1f);
		}
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
		print ("slowing down");
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
}
