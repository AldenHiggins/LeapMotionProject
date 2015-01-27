using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLogic : MonoBehaviour 
{
	public GameObject avatarPrefab;
	public GameObject thisPlayer;
	public GameObject thisCamera;
	public GameObject fireBall;
	public GameObject floor;
	public Vector3 position = new Vector3 (0f,1f,-5.0f);
	public Vector3 normal = new Vector3(0f,1f,0f);
	public float radius = 24.0f;

	public HandController handController = null;

	private Dictionary<int, GameObject> projectiles;
	private NetworkView view;
	private bool fireballCharged;
	private bool isBlocking;

	private GameObject playerAvatar;
	private int fireballTimer;
	private int blockTimer;
	private Networking network;

	// Use this for initialization
	void Start () 
	{
		view = gameObject.networkView;
		fireballCharged = false;
		isBlocking = false;
		fireballTimer = 0;
		blockTimer = 0;
		projectiles = new Dictionary<int, GameObject> ();
		network = (Networking) gameObject.GetComponent (typeof(Networking));
	}
	
	// Update is called once per frame
	void Update () 
	{
		//print (thisPlayer.transform.position);
		HandModel[] hands = handController.GetAllGraphicsHands();
		if (hands.Length > 0)
		{
			Vector3 direction0 = (hands[0].GetPalmPosition() - handController.transform.position).normalized;
			Vector3 normal0 = hands[0].GetPalmNormal().normalized;

//			print ("Normal 0: " + normal0);
//			print ("Dot product: " + Vector3.Dot (normal0, thisCamera.transform.forward));

			//  -.6 or less means the palm is facing the camera
			if (Vector3.Dot (normal0, thisCamera.transform.forward) < -.6 && !fireballCharged)
			{
				fireballCharged = true;
			}

			// .6 or more means the palm is facing away from the camera
			if (Vector3.Dot (normal0, thisCamera.transform.forward) > .6)
			{
				blockTimer++;
				if (blockTimer > 40 && !isBlocking)
				{
					print ("Player is blocking");
					isBlocking = true;
				}
			}
			else
			{
				if (isBlocking)
				{
					isBlocking = false;
					print ("Player not blocking any more!");
				}
				blockTimer = 0;
			}

			// .6 or more means the palm is facing away from the camera
			if (Vector3.Dot (normal0, thisCamera.transform.forward) > .6 && fireballCharged)
			{
				fireballCharged = false;
				// Make sure the fireball spawns in front of the player at a reasonable distance
				Vector3 spawnPosition = hands[0].GetPalmPosition();
				spawnPosition += new Vector3(thisCamera.transform.forward.normalized.x * 3.8f, thisCamera.transform.forward.normalized.y * 3.8f, thisCamera.transform.forward.normalized.z * 3.8f);
				// Scale the fireball's velocity
				Vector3 startingVelocity = thisCamera.transform.forward.normalized;
				startingVelocity *= .2f;
				// Generate a hash value for the fireball (for network synchronization)
				int fireballHash = generateProjectileHash();

				createFireball(spawnPosition, thisCamera.transform.rotation, startingVelocity, fireballHash);
				view.RPC ("makeFireballNetwork", RPCMode.Others, spawnPosition, thisCamera.transform.rotation, startingVelocity, fireballHash);
			}
		}
//		else if (hands.Length > 1)
//		{
//			Vector3 direction0 = (hands[0].GetPalmPosition() - handController.transform.position).normalized;
//			Vector3 normal0 = hands[0].GetPalmNormal().normalized;
//			
//			Vector3 direction1 = (hands[1].GetPalmPosition() - handController.transform.position).normalized;
//			Vector3 normal1 = hands[1].GetPalmNormal().normalized;
//			
//			print ("Normal 0: " + normal0);
//			print ("Normal 1: " + normal1);
//
//		}

		// Only make the fireball if you're the server for synchronization purposes (so the two fireballs generated at the same time
		// won't have different hashes)
		if (network.isServer)
		{
			// Temp fireball launcher to test
			fireballTimer++;
			if (fireballTimer > 300)
			{
				fireballTimer = 0;
				int hash = generateProjectileHash();
				createFireball(new Vector3(-4.6f, 76.75f, 1.8f), Quaternion.identity, new Vector3(0.0f, 0.0f, -0.1f), hash);
				view.RPC ("makeFireballNetwork", RPCMode.Others, new Vector3(-4.6f, 76.75f, 1.8f), Quaternion.identity, new Vector3(0.0f, 0.0f, -0.1f), hash);
			}
		}
	}

	[RPC]
	public void makeFireballNetwork(Vector3 position, Quaternion rotation, Vector3 velocity, int hashValue)
	{
		print ("Remote fireball called!");
		createFireball (position, rotation, velocity, hashValue);
	}

	public void createFireball(Vector3 position, Quaternion rotation, Vector3 velocity, int hashValue)
	{
		GameObject newFireball = (GameObject) Instantiate(fireBall, position, rotation);
		MoveFireball moveThis = (MoveFireball) newFireball.GetComponent(typeof(MoveFireball));
		moveThis.setVelocity(velocity);
		newFireball.renderer.enabled = true;
		moveThis.setHash (hashValue);

		// Now add newly generated fireball to projectiles dictionary
		projectiles.Add (hashValue, newFireball);
	}

	public void reverseProjectileOnOtherClients(int hashValue)
	{
		view.RPC ("reverseFireball", RPCMode.Others, hashValue);
	}

	[RPC]
	public void reverseFireball(int fireballHash)
	{
		GameObject fireball = projectiles [fireballHash];
		print ("Got fireball: " + fireball.gameObject.name);
		MoveFireball fireballScript = (MoveFireball) fireball.GetComponent(typeof(MoveFireball));
		fireballScript.reverseVelocity ();
	}

	public bool isPlayerBlocking ()
	{
		return isBlocking;
	}


	public void createNewPlayer ()
	{
		view.RPC ("makePlayerOnClient", RPCMode.Others);
	}

	[RPC]
	public void makePlayerOnClient ()
	{
		print ("Remote procedure called!");
		makePlayerOnClientHelper ();
	}

	public void makePlayerOnClientHelper()
	{
		// previously was Network.Instantiate
		playerAvatar = (GameObject) Network.Instantiate (avatarPrefab, thisPlayer.transform.position, thisPlayer.transform.rotation, 1);
		MoveAvatar avatar = (MoveAvatar) playerAvatar.GetComponent (typeof(MoveAvatar));
		avatar.setPlayer (thisPlayer);
		avatar.hidePlayer ();	
	}


	public Vector3 RandomPointOnPlane()
	{
		Vector3 randomPoint;

		do
		{
			randomPoint = Vector3.Cross(Random.insideUnitSphere, normal);
		} while (randomPoint == Vector3.zero);

		randomPoint.Normalize();
		randomPoint *= radius;
		randomPoint += position;

		return randomPoint;
//		return new Vector3 (0,1,0);
	}

	private int generateProjectileHash()
	{
		int fireballHash = 0;
		do
		{
			fireballHash = Random.Range(0, 10000);
		} while(projectiles.ContainsKey(fireballHash));

		return fireballHash;
	}
}
