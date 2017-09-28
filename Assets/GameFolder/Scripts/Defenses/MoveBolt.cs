using UnityEngine;
using System.Collections;

public class MoveBolt : MonoBehaviour 
{
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

	void OnCollisionEnter(Collision collision) 
	{
		if (collision.gameObject == null) 
		{
			return;
		}

		GameObject other = collision.gameObject;

		IUnit enemy = (IUnit) other.GetComponent(typeof(IUnit));	
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

		}
		// Collide with an enemy
		else if (enemy != null)
		{
			enemy.dealDamage(damage, Vector3.zero);
			Destroy (gameObject);
		}
		else
		{
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
