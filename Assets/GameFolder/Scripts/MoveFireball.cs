using UnityEngine;
using System.Collections;

public class MoveFireball : MonoBehaviour {
	private Vector3 velocity;
	public GameLogic game;
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
			gameObject.transform.position += velocity;
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


	void OnCollisionEnter (Collision col)
	{
		// Check if a player or their leap motion hands are hit
		if (col.gameObject.name == "LeapOVRPlayerController" || col.gameObject.name == "palm" || col.gameObject.name == "bone1" || col.gameObject.name == "bone2")
		{
			if (!game.isPlayerBlocking())
			{
				PlayerLogic hitPlayer = (PlayerLogic) col.gameObject.GetComponent(typeof(PlayerLogic));
				hitPlayer.respawn();
			}
			// Reflect back blocked fireballs
			else
			{
				reverseVelocity();
				game.reverseProjectileOnOtherClients(hashValue);
			}
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
}
