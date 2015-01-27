﻿using UnityEngine;
using System.Collections;

public class MoveFireball : MonoBehaviour {
	private Vector3 velocity;
	public GameLogic game;

	// Use this for initialization
	void Start () 
	{
		
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
				print("Fireball blocked!");
				velocity *= -1.0f;
			}
		}
		else
		{
			Destroy(gameObject);
		}
	}
}