using UnityEngine;
using System.Collections;

public class BasicEnemyController : MonoBehaviour 
{
	public GameLogic game;
	public float speed;
	public int startingHealth;
	public float attackRadius;
	public int attackDamage;

	private Vector3 velocity;
	private Animator anim;
	private int health;
	private bool attacking;
	private NavMeshAgent agent;
	private GameObject target;

	// Use this for initialization
	void Start () 
	{
		attacking = false;
		anim = transform.GetChild (0).gameObject.GetComponent<Animator> ();
		health = startingHealth;
		agent = gameObject.GetComponent<NavMeshAgent> ();
		target = game.getEnemyTarget ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (health < 0)
			return;


//		print ("Current velocity: " + rigidbody.velocity);

		velocity = target.transform.position - transform.position;
		velocity.y = 0.0f;
		// If the enemy is outside melee range keep coming forward
		if (velocity.magnitude > attackRadius)
		{
//			print("Running");
			if (attacking == false)
			{
				anim.SetBool ("Running", true);
				velocity.Normalize ();
				velocity *= speed;
				if (agent.enabled == true)
					agent.SetDestination (target.transform.position);
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
				agent.Stop();
				StartCoroutine(attack());
			}
		}
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
		yield return new WaitForSeconds (anim.GetCurrentAnimationClipState(0)[0].clip.length - .1f);

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
		print ("Dealing damage, current health: " + health);
		health -= damage;
		if (health > 0)
		{
			print ("Wounding by : " + damage + " amount");	
			anim.Play ("wound");
		}
		else
		{
			StartCoroutine(kill());
		}
	}

	public void applyForce(Vector3 force)
	{
		rigidbody.AddForce (force, ForceMode.Impulse);
		agent.enabled = false;
		StartCoroutine (restartAgent ());
	}

	public void applyExplosiveForce(float force, Vector3 position, float radius)
	{
		rigidbody.AddExplosionForce (force, position, radius, 20.0f, ForceMode.Impulse);
		agent.enabled = false;
		StartCoroutine (restartAgent ());
	}

	IEnumerator restartAgent ()
	{
		while (rigidbody.velocity.magnitude > 0.1)
		{
			yield return new WaitForSeconds(.1f);
		}
		print ("Restarting agent");
		agent.enabled = true;
	}


	IEnumerator kill()
	{
		anim.Play ("death");
		while(!anim.GetCurrentAnimatorStateInfo(0).IsName("death"))
		{
			yield return new WaitForSeconds(.1f);
		}
		yield return new WaitForSeconds (anim.GetCurrentAnimationClipState(0)[0].clip.length);
		Destroy (this.gameObject);
	}


}
