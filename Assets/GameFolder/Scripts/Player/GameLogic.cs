using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameLogic : MonoBehaviour 
{
	// PUBLIC GAME OBJECTS
	public GameObject avatarPrefab;
	public GameObject thisPlayer;
	public GameObject thisCamera;
	public GameObject goalPosition;
	public bool disableMovement;
	// Player HUD
	public GameObject playerHud;
	// ENEMY SPAWNERS
	public GameObject enemySpawners;
	public GameObject spawnedEnemies;
	// PLAYER ATTACKS
	public GameObject fireBall;
	public HandController handController = null;

	// TURRET PLACEMENT HUD
	public GameObject turretHud;
	public ButtonDemoGraphics callForWaveButtonGraphic;
	public ButtonDemoToggle callForWaveButton;

	// LEVEL ROUNDS
	public ButtonDemoGraphics roundButton;
	public ButtonDemoToggle nextRoundButton;
	public UIFollowPlayer endRoundScreen;
	public GameObject winScreen;
	private float currentRoundTime = 0;
	private int currentRound = 0;
	public Text roundTimerText;
	public Text roundText;
	private bool nextRound = false;
	private bool roundActive = false;
	// MONSTER WAVES
	public GameObject waveContainer;
	private MonsterWave[] enemyWaves;
	// INTERNAL VARIABLES
	private GameObject playerAvatar;
	private NetworkView view;
	private Networking network;
	private PlayerLogic playerLogic;
	private DefensiveAbilities defensiveAbilities;
	private OffensiveAbilities offensiveAbilities;
	// GAME CONTROLLER VARIABLES
	private bool previousSwitchPressed = false;
	private bool fireballCharged;
	private bool isBlocking;

	// FIreball timing variable.
	private int fireballTimer;
	// HEAD BASED MOVEMENT
	private HMDMovement hmdMovement;

	// RUNTIME GAME REPRESENTATION
	private Dictionary<int, GameObject> projectiles;

	// DEFENSIVE STAGE
	private bool isDefensiveStageActive = false;


	// ALL THE ATTACKS
	public AAttack placeTurretAttack;
	public AAttack placeBallistaAttack;
	public AAttack placeOilSlickAttack;
	public AAttack fireballAttack;
	public AAttack tornadoAttack;
	public AAttack flameThrowerAttack;
	public AAttack iceBallAttack;

	// Initialize variables
	void Start () 
	{
		view = gameObject.GetComponent<NetworkView>();
		fireballCharged = false;
		isBlocking = false;
		fireballTimer = 0;
		projectiles = new Dictionary<int, GameObject> ();
		network = (Networking) gameObject.GetComponent (typeof(Networking));
		playerLogic = (PlayerLogic) thisPlayer.GetComponent (typeof(PlayerLogic));
		hmdMovement = (HMDMovement)thisPlayer.GetComponent (typeof(HMDMovement));
		offensiveAbilities = (OffensiveAbilities) gameObject.GetComponent (typeof(OffensiveAbilities));
		defensiveAbilities = (DefensiveAbilities) gameObject.GetComponent (typeof(DefensiveAbilities));
		defensiveAbilities.showHideTurretPositions (false);
		// Disable leap motion for defensive player
		if (playerLogic.isDefensivePlayer)
		{
			HandController hand = (HandController) thisPlayer.transform.GetChild (1).GetChild (1).
				GetChild (0).gameObject.GetComponent(typeof(HandController));
			hand.enabled = false;

			// Make the player not use gravity if they are defensive
			OVRPlayerController ovrController = (OVRPlayerController) thisPlayer.GetComponent(typeof(OVRPlayerController));
//			ovrController.changeGravityUse(false);
		}

		// Initialize waves
		enemyWaves = new MonsterWave[waveContainer.transform.childCount];
		for (int i = 0; i < waveContainer.transform.childCount; i++)
		{
			enemyWaves[i] = (MonsterWave) waveContainer.transform.GetChild(i).gameObject.GetComponent(typeof(MonsterWave));
		}
		// Start up the round timer
		StartCoroutine (roundFunction());
	}

	// Control loop to check for player input
	void Update () 
	{
		// Check if round is active in order for players to use their abilities
		if (roundActive || isDefensiveStageActive)
		{
			if (!playerLogic.isDefensivePlayer)
			{
				offensiveAbilities.controlCheck();
			}
//			else
//			{
//				defensiveAbilities.controlCheck();
//			}
		}

		// Shared abilities
		bool switchButtonPressed = OVRGamepadController.GPC_GetButton (OVRGamepadController.Button.Back);

		if (!switchButtonPressed && previousSwitchPressed)
		{
			print ("Switching is happening");
			playerLogic.switchOffensiveDefensive ();
		}

		previousSwitchPressed = switchButtonPressed;

		// Debugging abilities
		//<--------------------- Z to fire fireball ------------------------------->	
		if (Input.GetKeyDown (KeyCode.Z)) 
		{
			playerCastFireball ();
		}		


		// Check for the new round button to be pressed
		if (roundButton.isPressed() || Input.GetKeyDown (KeyCode.V))
		{
			nextRound = true;
			nextRoundButton.ButtonTurnsOff();
			// Temporarily skip turret placement phase
			// callForWaveButton.ButtonTurnsOn();
		}

		// Update round timer
		if (roundActive)
		{
			currentRoundTime -= Time.deltaTime;
			if (currentRoundTime < 0)
			{
				roundTimerText.text = "0";
			}
			else
			{
				roundTimerText.text = "" + currentRoundTime.ToString("F2");
			}
		}
	}

	IEnumerator roundFunction()
	{
		for (int i = 0; i < enemyWaves.Length; i++)
		{
			// Set the round text
			roundText.text = "ROUND " + (i + 1);
			// Present start round screen and wait
			playerHud.SetActive(false);
			endRoundScreen.enableUI();

			// Enable hand controller for defensive player
			if (playerLogic.isDefensivePlayer)
			{
				HandController hand = (HandController) thisPlayer.transform.GetChild (1).GetChild (1).
					GetChild (0).gameObject.GetComponent(typeof(HandController));
				hand.enabled = true;
			}

			// Now wait for the player to press next round button
			while (!nextRound)
			{
				yield return new WaitForSeconds(.2f);
			}

			print ("Activating the next round");

			// Remove attack selection screen
			endRoundScreen.disableUI();

			// Start defensive setup phase
			isDefensiveStageActive = true;
			turretHud.SetActive(true);
			// Wait a couple of seconds for the player to readjust.
			yield return new WaitForSeconds(1f);

			defensiveAbilities.updateDefencesCostText();
			//defensiveAbilities.showHideTurretPositions(true);
			offensiveAbilities.handFlipAttack = placeBallistaAttack;
			offensiveAbilities.fistAttack = placeOilSlickAttack;

			// Enable HMD movement and reset position if enabled
			if(!disableMovement)
			{
				hmdMovement.enabled = true;
				hmdMovement.resetPosition();
			}
			else
			{
				hmdMovement.enabled = false;
			}

			// Wait for player to end defensive setup phase
			while (!callForWaveButtonGraphic.isPressed()) 
			{
				yield return new WaitForSeconds(.2f);
			}

			offensiveAbilities.handFlipAttack.inactiveFunction();
			offensiveAbilities.fistAttack.inactiveFunction();
			// Clean up defensive setup stuff
			callForWaveButton.ButtonTurnsOff();
			//defensiveAbilities.showHideTurretPositions(false);
			turretHud.SetActive(false);
			isDefensiveStageActive = false;

			// Change the hand flip attack to fireball
			offensiveAbilities.handFlipAttack = fireballAttack;
			offensiveAbilities.fistAttack = iceBallAttack;

			// Start the next round, spawn enemies, wait for the timer
			nextRound = false;
			roundActive = true;
			currentRoundTime = enemyWaves[i].roundTime;
			// Enable the player HUD
			playerHud.SetActive(true);

		
			// Start the enemy spawners
			enemyWaves[i].startWave ();
			// Wait for the round to time out
			yield return new WaitForSeconds(currentRoundTime);

			// Turn off spawners
//			enableDisableSpawners(false);

			// Wait for all the enemies to be cleared
			while (spawnedEnemies.transform.childCount > 0)
			{
				yield return new WaitForSeconds(.3f);
			}

			// Deactivate round
			roundActive = false;
			playerLogic.changeCurrency(500);
			hmdMovement.enabled = false;
		}

		// The game/map is over, display end game screen
		winScreen.SetActive(true);
	}

	public void enableDisableSpawners(bool enableOrDisable)
	{
//		for (int i = 0; i < enemySpawners.transform.childCount; i++)
//		{
//
//			GameObject spawner = enemySpawners.transform.GetChild(i).gameObject;
//			EnemySpawner spawnScript = (EnemySpawner) spawner.GetComponent(typeof(EnemySpawner));
//			if (enableOrDisable)
//			{
//				spawnScript.startSpawning();
//			}
//			else
//			{
//				spawnScript.stopSpawning();
//			}
//		}
	}

	public void createFireball(Vector3 position, Quaternion rotation, Vector3 velocity, int hashValue)
	{
		GameObject newFireball = (GameObject) Instantiate(fireBall, position, rotation);
		newFireball.SetActive(true); 
//		MoveFireball moveThis = (MoveFireball) newFireball.GetComponent(typeof(MoveFireball));
//		moveThis.setVelocity(velocity);
//		newFireball.GetComponent<Renderer>().enabled = true;
//		moveThis.setHash (hashValue);

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

	public bool isPlayerBlocking ()
	{
		return isBlocking;
	}

	public GameObject getEnemyTarget()
	{
		if (playerLogic.isDefensivePlayer)
		{
			return goalPosition;
		}
//		return thisPlayer;
		return goalPosition;
	}

	public RaycastHit getRayHit()
	{
		int maskOne = 1 << 10;
		int maskTwo = 1 << 11;
		int mask = maskOne | maskTwo;
		Ray ray = new Ray (thisCamera.transform.position, thisCamera.transform.forward);
		RaycastHit hit;
		Physics.Raycast (ray, out hit, 100f, mask);
		return hit;
	}

	[RPC]
	public void makeFireballNetwork(Vector3 position, Quaternion rotation, Vector3 velocity, int hashValue)
	{
		createFireball (position, rotation, velocity, hashValue);
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

	/*
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
	public void createNewPlayer ()
	{
		view.RPC ("makePlayerOnClient", RPCMode.Others);
	}
	[RPC]
	public void reverseFireball(int fireballHash)
	{
		GameObject fireball = projectiles [fireballHash];
		print ("Got fireball: " + fireball.gameObject.name);
		MoveFireball fireballScript = (MoveFireball) fireball.GetComponent(typeof(MoveFireball));
		fireballScript.reverseVelocity ();
	}

	public void reverseProjectileOnOtherClients(int hashValue)
	{
		view.RPC ("reverseFireball", RPCMode.Others, hashValue);
	}

}
*/