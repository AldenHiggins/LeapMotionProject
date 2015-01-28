using UnityEngine;
using System.Collections;

public class BasicEnemyController : MonoBehaviour {
	public GameObject player;

	private Vector3 velocity;
	private Animator anim;
	private int health;

	// Use this for initialization
	void Start () 
	{
		anim = GetComponent<Animator> ();
		health = 20;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (health < 0)
			return;
		velocity = player.transform.position - transform.position;
		velocity.y = 0.0f;
		if (velocity.magnitude > 2)
		{
			anim.SetBool ("Running", true);
			velocity.Normalize ();
			velocity *= .02f;
			transform.position += velocity;
			// Face the target as well
			transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position);
		}
		else
		{
			anim.SetBool ("Running", false);
		}
	}


	public void dealDamage(int damage)
	{
		print ("Dealing damage");
		health -= damage;
		anim.Play ("wound");
		if (health < 0)
		{
//			StartCoroutine(kill());
//			anim.Play ("death");
			Destroy (this.gameObject);
		}
	}

//	IEnumerator kill()
//	{
//		anim.Play ("death");
//		Animation anim = anim.
//		yield return new WaitForSeconds (anim.animation["death"].length);
//	}


}
