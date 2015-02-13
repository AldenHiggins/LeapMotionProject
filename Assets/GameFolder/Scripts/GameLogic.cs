using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLogic : MonoBehaviour 
{
	// PUBLIC GAME OBJECTS
	public GameObject avatarPrefab;
	public GameObject thisPlayer;
	public GameObject thisCamera;
	public GameObject goalPosition;
	public Vector3 position = new Vector3 (0f,1f,-5.0f);
	public Vector3 normal = new Vector3(0f,1f,0f);
	public float radius = 24.0f;
	public HandController handController = null;
	// PLAYER ATTACKS
	public GameObject fireBall;
	public GameObject clapProjectile;
	// TURRETS
	public GameObject cTurret;
	public GameObject turretPositions;
	// OIL MATERIALS
	public GameObject oilSlick;
	// INTERNAL VARIABLES
	private NetworkView view;
	private bool fireballCharged;
	private bool isBlocking;
	private GameObject playerAvatar;
	private int fireballTimer;
	private Networking network;
	private PlayerLogic playerLogic;
	private bool previousTurretButtonPressed;

	// GAME CONTROLLER VARIABLES
	private bool previousXDown = false;

	// RUNTIME GAME REPRESENTATION
	private Dictionary<int, GameObject> projectiles;


	// Initialize variables
	void Start () 
	{
		showHideTurretPositions (false);
		view = gameObject.networkView;
		fireballCharged = false;
		isBlocking = false;
		previousTurretButtonPressed = false;
		fireballTimer = 0;
		projectiles = new Dictionary<int, GameObject> ();
		network = (Networking) gameObject.GetComponent (typeof(Networking));
		playerLogic = (PlayerLogic) thisPlayer.GetComponent (typeof(PlayerLogic));
	}
	
	// Control loop to check for player input
	void Update () 
	{
		HandModel[] hands = handController.GetAllGraphicsHands();
		if (hands.Length == 1)
		{
			Vector3 direction0 = (hands[0].GetPalmPosition() - handController.transform.position).normalized;
			Vector3 normal0 = hands[0].GetPalmNormal().normalized;

			//  Charge a fireball, -.6 or less means the palm is facing the camera
			if (Vector3.Dot (normal0, thisCamera.transform.forward) < -.6 && !fireballCharged)
			{
				fireballCharged = true;
			}

			// Fire a fireball, .6 or more means the palm is facing away from the camera
			if (Vector3.Dot (normal0, thisCamera.transform.forward) > .6 && fireballCharged)
			{
				fireballCharged = false;
				// First check if the player has enough energy
				if (playerLogic.getEnergy() > 10)
				{
					playerCastFireball();
				}
			}
		}
		else if (hands.Length > 1)
		{
			Vector3 direction0 = (hands[0].GetPalmPosition() - handController.transform.position).normalized;
			Vector3 normal0 = hands[0].GetPalmNormal().normalized;
			
			Vector3 direction1 = (hands[1].GetPalmPosition() - handController.transform.position).normalized;
			Vector3 normal1 = hands[1].GetPalmNormal().normalized;
			
			//  Check for and perform a clap attack
			if (Vector3.Dot (normal0, normal1) < -.6)
			{
				Vector3 distance = hands[0].GetPalmPosition() - hands[1].GetPalmPosition();
				if (distance.magnitude < .09)
				{
					clapAttack (thisPlayer.transform.position + new Vector3(0.0f, 0.7f, 0.0f));
				}
			}
		}

		
		//<--------------------- Z to fire fireball ------------------------------->	
		if (Input.GetKeyDown(KeyCode.Z)) 
		{
			playerCastFireball();
		}

		if(playerLogic.isDefensivePlayer){
			//<--------------------- CONTROLLER X TO DROP OIL SLICK ----------------------->

			//Bomb Projectile
			bool xPressed = OVRGamepadController.GPC_GetButton(OVRGamepadController.Button.X);
			
			// Create projectile
			if (xPressed && !previousXDown)
			{
				print("X is pressed");
				oilSlick = (GameObject) Instantiate (oilSlick);
			}
			// Animate projectile in front of player
			else if (xPressed)
			{
				RaycastHit hit = getRayHit();
				oilSlick.transform.position = hit.point;
				//oilSlick.transform.rotation Quaternion.identity);
			}
			// Fire projectile
			else if (!xPressed && previousXDown)
			{
				RaycastHit hit = getRayHit();
				oilSlick.transform.position = hit.point;
				OilSlick oilSlickScript = (OilSlick) oilSlick.GetComponent(typeof(OilSlick));
				oilSlickScript.enabled = true;
				print ("placed it at " + hit.point.y);
			}
			
			previousXDown = xPressed;


			//<--------------------- X to create explosion ------------------------------->	
			if (Input.GetKeyDown(KeyCode.X)) 
			{
				RaycastHit hit = getRayHit();
				Instantiate (clapProjectile, hit.point + new Vector3(0.0f, 2.0f, 0.0f), Quaternion.identity);
			}

		
			//<--------------------- C to place turret ------------------------------->
			if (playerLogic.isDefensivePlayer && Input.GetKey(KeyCode.V)) 
			{
				// Display prospective turret spots
				showHideTurretPositions(true);
			}
			else if (playerLogic.isDefensivePlayer && !Input.GetKey(KeyCode.V) && previousTurretButtonPressed)
			{
				showHideTurretPositions(false);

				int mask = 1 << 10;
				Ray ray = new Ray(thisCamera.transform.position,thisCamera.transform.forward);
				RaycastHit hit;
				Physics.Raycast(ray, out hit, 100f, mask);
				float minDistance = float.MaxValue;
				GameObject closestTurret = null;
				for (int i = 0; i < turretPositions.transform.childCount; i++)
				{
					GameObject turretPos = turretPositions.transform.GetChild (i).gameObject;
					float distance = Vector3.Distance(turretPos.transform.position, hit.point);
					if (distance < minDistance)
					{
						minDistance = distance;
						closestTurret = turretPos;
					}
				}

				if (closestTurret != null)
				{
					Instantiate (cTurret, closestTurret.transform.position, closestTurret.transform.rotation);
					Destroy (closestTurret);
				}
				else
				{
					print ("Could not place a turret, no more locations available!");
				}

			}
			previousTurretButtonPressed = Input.GetKey (KeyCode.V);
		}
	}

	public void clapAttack(Vector3 position)
	{
		Instantiate (clapProjectile, position, Quaternion.identity);
	}

	[RPC]
	public void makeFireballNetwork(Vector3 position, Quaternion rotation, Vector3 velocity, int hashValue)
	{
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

	public void playerCastFireball()
	{
		// Have the player spend mana
		playerLogic.useEnergy(10);
		// Make sure the fireball spawns in front of the player at a reasonable distance
		Vector3 spawnPosition = thisCamera.transform.position;
		spawnPosition += new Vector3(thisCamera.transform.forward.normalized.x * .8f, thisCamera.transform.forward.normalized.y * .8f, thisCamera.transform.forward.normalized.z * .8f);
		// Scale the fireball's velocity
		Vector3 startingVelocity = thisCamera.transform.forward.normalized;
		startingVelocity *= .2f;
		// Generate a hash value for the fireball (for network synchronization)
		int fireballHash = generateProjectileHash();
		
		createFireball(spawnPosition, thisCamera.transform.rotation, startingVelocity, fireballHash);
		view.RPC ("makeFireballNetwork", RPCMode.Others, spawnPosition, thisCamera.transform.rotation, startingVelocity, fireballHash);
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


	public GameObject getEnemyTarget()
	{
		if (playerLogic.isDefensivePlayer)
		{
			return goalPosition;
		}
		return thisPlayer;
	}

	public void showHideTurretPositions(bool showOrHide)
	{
		// Don't display any of the turret positions
		for (int i = 0; i < turretPositions.transform.childCount; i++)
		{
			GameObject turretPos = turretPositions.transform.GetChild (i).gameObject;
			turretPos.transform.GetChild (0).gameObject.renderer.enabled = showOrHide;
			turretPos.transform.GetChild (1).gameObject.renderer.enabled = showOrHide;
		}
	}

	private RaycastHit getRayHit(){
		int mask = 1 << 10;
		Ray ray = new Ray (thisCamera.transform.position, thisCamera.transform.forward);
		RaycastHit hit;
		Physics.Raycast (ray, out hit, 100f, mask);
		return hit;
	}
}
