using UnityEngine;
using System.Collections;

public class BasicEnemyController : MonoBehaviour {
	public GameObject player;
	public float speed;
	public int startingHealth;
	public float attackRadius;
	public int attackDamage;

	private Vector3 velocity;
	private Animator anim;
	private int health;
	private bool attacking;

	// Use this for initialization
	void Start () 
	{
		attacking = false;
		anim = GetComponent<Animator> ();
		health = startingHealth;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (health < 0)
			return;
		velocity = player.transform.position - transform.position;
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
				transform.position += velocity;
				// Face the target as well
				transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position);
			}
		}
		// If the player is in range attack him
		else
		{
			anim.SetBool ("Running", false);
			if (attacking == false)
			{
				attacking = true;
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

		Vector3 distance = player.transform.position - transform.position;
		distance.y = 0.0f;
		if (distance.magnitude < attackRadius)
		{
			PlayerLogic playerToAttack = (PlayerLogic) player.GetComponent(typeof(PlayerLogic));
			playerToAttack.dealDamage(attackDamage);
		}
		attacking = false;
		anim.SetBool ("Attacking", false);
	}



	public void dealDamage(int damage)
	{
//		print ("Dealing damage");
		if (health >= 0)
		{
			health -= damage;
			anim.Play ("wound");
		}
		if (health < 0)
		{
			StartCoroutine(kill());
		}
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
