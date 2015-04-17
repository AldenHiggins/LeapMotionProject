using UnityEngine;
using System.Collections;

public class BasicEnemyController : MonoBehaviour 
{
	public GameLogic game;
	public PlayerLogic player;
	public float speed;
	public int startingHealth;
	public float attackRadius;
	public int attackDamage;
	public int currencyOnKill;
	public int livesTakenOnGoalReached;
	public bool showNavMeshPath;

	public GameObject pathLine;

	private GameObject thisPathLine;


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
	// RAGDOLL
	public float ragdollTime;
	public float ragdollForceFactor;
	public GameObject ragDollCenterObject;
	public bool usesRagdoll;


	// Use this for initialization
	void Start () 
	{
		attacking = false;
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
//		target = game.getEnemyTarget ();
		target = player.gameObject;
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
		if (health < 0)
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

				if (!usesRootMotion)
				{
					if (agent.enabled == true && !agent.hasPath)
					{
						agent.SetDestination (target.transform.position);
					}
				}

					
//				transform.position += velocity;
//				rigidbody.AddForce (velocity, ForceMode.VelocityChange);

				// Face the target as well
				transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);
				transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
			}
		}
		// If the player is in range attack him
		else
		{
			anim.SetBool ("Running", false);
			if (attacking == false)
			{
				attacking = true;
				if (!usesRootMotion)
				{
					agent.Stop();
				}
				StartCoroutine(attack());
			}
		}

		if (showNavMeshPath)
			displayNavMeshPath ();
	}

	IEnumerator attack()
	{
//		print ("Attacking");
		anim.SetBool ("Attacking", true);
		while(!anim.GetCurrentAnimatorStateInfo(0).IsName("attack0"))
		{
			yield return new WaitForSeconds(.01f);
		}
//		print ("Animation time: " + anim.GetCurrentAnimationClipState (0) [0].clip.length);
		yield return new WaitForSeconds (anim.GetCurrentAnimatorClipInfo(0)[0].clip.length - .1f);

		Vector3 distance = target.transform.position - transform.position;
		distance.y = 0.0f;
		if (distance.magnitude < attackRadius)
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
			if (usesRagdoll)
			{
				StartCoroutine(ragdollKill());
			}
			else
			{
				StartCoroutine(kill());
			}
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
		print ("Restarting agent");
		agent.enabled = true;
//		rigidbody.isKinematic = true;
	}


	IEnumerator kill()
	{
		if (agent.enabled)
		{
			agent.enabled = false;
		}
		player.changeCurrency (currencyOnKill);
		anim.Play ("death");
		while(!anim.GetCurrentAnimatorStateInfo(0).IsName("death"))
		{
			yield return new WaitForSeconds(.1f);
		}
		yield return new WaitForSeconds (anim.GetCurrentAnimatorClipInfo(0)[0].clip.length);
		Destroy (this.gameObject);
	}

	IEnumerator ragdollKill()
	{
//		agent.enabled = false;
		anim.enabled = false;
		print ("Made ragdoll");
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

//	void OnAnimatorMove ()
//	{
//		//only perform if walking
//		if (anim.GetCurrentAnimatorStateInfo(0).IsName("run"))
//		{
//			//set the navAgent's velocity to the velocity of the animation clip currently playing
//			agent.velocity = anim.deltaPosition / Time.deltaTime;
//			
//			//set the rotation in the direction of movement
//			transform.rotation = Quaternion.LookRotation(agent.desiredVelocity);
//		}
//	}

}
