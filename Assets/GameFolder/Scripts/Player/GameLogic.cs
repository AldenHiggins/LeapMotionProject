using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameLogic : MonoBehaviour 
{
	// PUBLIC GAME OBJECTS
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
    //public HandController handController = null;

	// AUDIO CLIPS
	public AudioSource mainGameMusic;
	public AudioSource defensivePhaseMusic;
	public AudioSource winningSound;
	public AudioSource losingSound;

	// TURRET PLACEMENT HUD
	public GameObject turretHud;
	public ButtonDemoGraphics callForWaveButtonGraphic;
	public ButtonDemoToggle callForWaveButton;

	// LEVEL ROUNDS
	public ButtonDemoGraphics roundButton;
	public ButtonDemoToggle nextRoundButton;
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
	int waveIndex;

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

	// HEAD BASED MOVEMENT
	private HMDMovement hmdMovement;

	// RUNTIME GAME REPRESENTATION
	private Dictionary<int, GameObject> projectiles;

	// DEFENSIVE STAGE
	private bool isDefensiveStageActive = false;
	// End GAME/PLAYER DEATH
	public GameObject endGameHud;
	public Text endGameText;
	public ButtonDemoGraphics retryButton;
	public ButtonDemoGraphics mainMenuButton;
	public Text roundsSurvivedText;

    // CONTROLLER ATTACK LOGIC
    public ControllerAbilities controllerAttacks;

	// ALL THE ATTACKS
	public AAttack placeTurretAttack;
	public AAttack placeBallistaAttack;
	public AAttack placeOilSlickAttack;
	public AAttack fireballAttack;
	public AAttack emptyAttack;
	public AAttack flameThrowerAttack;
	public AAttack iceBallAttack;

	// STEAM ATTACKS
//	public RayCastTest steamManager;

	public SteamAttacks steamDefensivePlacement;
	public SteamAttacks steamEmptyAttack;
	public SteamAttacks steamFireballAttack;
	public SteamAttacks steamDefensiveSwitching;

	// STEAM ACTIVATE OFFENSE
	public bool startRound1;
	public bool startRound2;

	// WAVE TO START AT
	public int waveToStartAt;

	// Initialize variables
	void Start () 
	{
		Debug.Log ("Starting game logic");

		waveIndex = 0;
		fireballCharged = false;
		isBlocking = false;
		projectiles = new Dictionary<int, GameObject> ();
		network = (Networking) gameObject.GetComponent (typeof(Networking));
		playerLogic = (PlayerLogic) thisPlayer.GetComponent (typeof(PlayerLogic));

		if (!disableMovement) 
		{
			hmdMovement = (HMDMovement)thisPlayer.GetComponent (typeof(HMDMovement));
		}

		Debug.Log ("Initializing offensive abilities");

		offensiveAbilities = (OffensiveAbilities) gameObject.GetComponent (typeof(OffensiveAbilities));
		defensiveAbilities = (DefensiveAbilities) gameObject.GetComponent (typeof(DefensiveAbilities));
        //controllerAttacks = (ControllerAbilities) gameObject.GetComponent(typeof(ControllerAbilities));
		defensiveAbilities.showHideTurretPositions (false);

		Debug.Log ("Starting waves!");

		// Initialize waves
		enemyWaves = new MonsterWave[waveContainer.transform.childCount];
		for (int i = 0; i < waveContainer.transform.childCount; i++)
		{
			enemyWaves[i] = (MonsterWave) waveContainer.transform.GetChild(i).gameObject.GetComponent(typeof(MonsterWave));
		}
		// Start up the round timer
		StartCoroutine (roundFunction());

		Debug.Log ("Waves started!");
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
		}


//		// Shared abilities
//		bool switchButtonPressed = OVRGamepadController.GPC_GetButton (OVRGamepadController.Button.Back);
//
//		if (!switchButtonPressed && previousSwitchPressed)
//		{
//			print ("Switching is happening");
//			playerLogic.switchOffensiveDefensive ();
//		}
//
//		previousSwitchPressed = switchButtonPressed;
//
//		// Debugging abilities
//		//<--------------------- Z to fire fireball ------------------------------->	
//		if (Input.GetKeyDown (KeyCode.Z)) 
//		{
//			playerCastFireball ();
//		}		
//
//
		// Check for developer/debug actions
		if (Input.GetKeyDown (KeyCode.V))
		{
			startRound();
		}

		if (Input.GetKeyDown(KeyCode.F))
	    {
		    startRound1 = true;
		    startRound2 = true;
		}

		if (Input.GetKeyDown (KeyCode.Y))
	    {
			Application.LoadLevel(0);
		}

		if (Input.GetKeyDown (KeyCode.Q))
		{
			GameObject wallToDestroy = GameObject.Find ("DefensiveWall(Clone)");
			Destroy (wallToDestroy);
		}

		if (Input.GetKeyDown (KeyCode.E))
		{
			for (int enemyIndex = 0; enemyIndex < spawnedEnemies.transform.childCount; enemyIndex++)
			{
				Destroy (spawnedEnemies.transform.GetChild(enemyIndex).gameObject);
			}
		}

//		// Reload the level if retry is pressed
//		if (retryButton.isPressed())
//		{
//			Application.LoadLevel(2);
//		}
//
//		// Load the main menu if quit to main menu is pressed
//		if (mainMenuButton.isPressed())
//		{
//			Application.LoadLevel(0);
//		}
//
//		// Update round timer
//		if (roundActive)
//		{
//			currentRoundTime -= Time.deltaTime;
//			if (currentRoundTime < 0)
//			{
//				roundTimerText.text = "0";
//			}
//			else
//			{
//				roundTimerText.text = "" + currentRoundTime.ToString("F2");
//			}
//		}
//
//		// Press r to recenter view
//		if(Input.GetKeyDown (KeyCode.R))
//		{
//			playerLogic.gameObject.transform.rotation = Quaternion.Euler (0.0f, 90.0f, 0.0f);
//			StartCoroutine(waitToEnableRoundScreen());
//		}
	}

	public void startRound()
	{
		if (isDefensiveStageActive)
		{
			startRound1 = true;
			startRound2 = true;
		}
		else
		{
			nextRound = true;
			nextRoundButton.ButtonTurnsOff();
		}
	}

	IEnumerator waitToEnableRoundScreen()
	{
		yield return new WaitForSeconds (.5f);
//		endRoundScreen.disableUI ();
//		endRoundScreen.enableUI ();

        //endRoundScreen.SetActive (false);
        //endRoundScreen.SetActive (true);
	}

	IEnumerator roundFunction()
	{

		for (int i = waveToStartAt; i < enemyWaves.Length; i++)
		{
			// Reset the player's health
			playerLogic.resetHealth();
			waveIndex++;
			// Set the round text
			if (i == (enemyWaves.Length - 1)) 
			{
				roundText.text = "FINAL ROUND! ";
			}
			else
			{
				roundText.text = "ROUND " + (i + 1);
			}
			// Present start round screen and wait
//			playerHud.SetActive(false);
//			endRoundScreen.enableUI();
            //endRoundScreen.SetActive(true);
			defensivePhaseMusic.Play();

			// Size the player up to giant-scale
			playerLogic.gameObject.transform.localScale= new Vector3(1.1f, 1.1f, 1.1f);
			playerLogic.gameObject.transform.position = new Vector3(0.0f, 27.0f, 0.0f);
			// Make the player move faster if movement is enabled
            //if (!disableMovement)
            //{
            //    hmdMovement.moveSpeed *= 12;
            //}

            // Disable abilities during the change spells/end round screen
//            offensiveAbilities.rightHandFlipAttack = offensiveAbilities.emptyAttack;
//            offensiveAbilities.leftHandFlipAttack = offensiveAbilities.emptyAttack;
//            offensiveAbilities.rightHandFistAttack = offensiveAbilities.emptyAttack;
//            offensiveAbilities.leftHandFistAttack = offensiveAbilities.emptyAttack;

            //controllerAttacks.rightTriggerAttack = controllerAttacks.emptyAttack;
            //controllerAttacks.leftTriggerAttack = controllerAttacks.emptyAttack;

			// Now wait for the player to press next round button
			while (!nextRound)
			{
				yield return new WaitForSeconds(.2f);
			}

//			print ("Activating the next round");

			// Remove attack selection screen
//			endRoundScreen.disableUI();
            //endRoundScreen.SetActive(false);

			// Start defensive setup phase
			isDefensiveStageActive = true;
			turretHud.SetActive(true);


//			// STEAM VR REMOVE ATTACKS
//			steamManager.firstControllerAttack = steamEmptyAttack;
//			steamManager.secondControllerAttack = steamEmptyAttack;


			
            


//			defensiveAbilities.updateDefencesCostText();


			// Wait a couple of seconds for the player to readjust.
			yield return new WaitForSeconds(1.5f);

            //controllerAttacks.rightTriggerAttack.inactiveFunction();

			//Activate defensive abilities
//			offensiveAbilities.rightHandFlipAttack = offensiveAbilities.rightHandDefensiveFlip;
			offensiveAbilities.leftHandFlipAttack = offensiveAbilities.leftHandDefensiveFlip;
			offensiveAbilities.rightHandFistAttack = offensiveAbilities.rightHandDefensiveFist;
			offensiveAbilities.leftHandFistAttack = offensiveAbilities.leftHandDefensiveFist;

            //controllerAttacks.rightTriggerAttack = controllerAttacks.placeDefense;
            //controllerAttacks.leftTriggerAttack = controllerAttacks.switchDefense;

//			// STEAM VR ACTIVATE DEFENSIVE ATTACKS
//			steamManager.firstControllerAttack = steamDefensivePlacement;
//			steamManager.secondControllerAttack = steamDefensiveSwitching;


//			offensiveAbilities.handFlipAttack = placeBallistaAttack;
//			offensiveAbilities.fistAttack = placeOilSlickAttack;

			// Enable HMD movement and reset position if enabled
            //if(!disableMovement)
            //{
            //    hmdMovement.enabled = true;
            //    if (i == waveToStartAt)
            //    {
            //        hmdMovement.resetPosition();
            //    }
            //}

			// Wait for player to end defensive setup phase
			while (!startRound1 || !startRound2) 
			{
				yield return new WaitForSeconds(.2f);
			}

			startRound1 = false;
			startRound2 = false;

			// Call the inactive functions to clear in progress defensive placements
            //offensiveAbilities.rightHandFlipAttack.inactiveFunction();
            //offensiveAbilities.leftHandFlipAttack.inactiveFunction();
            //offensiveAbilities.rightHandFistAttack.inactiveFunction();
            //offensiveAbilities.leftHandFistAttack.inactiveFunction();

            //controllerAttacks.rightTriggerAttack.inactiveFunction();

//			offensiveAbilities.fistAttack.inactiveFunction();
			// Clean up defensive setup stuff
			callForWaveButton.ButtonTurnsOff();

			//defensiveAbilities.showHideTurretPositions(false);
			turretHud.SetActive(false);
			isDefensiveStageActive = false;

//			// STEAM VR ACTIVATE OFFENSIVE ABILITIES
//			steamManager.firstControllerAttack = steamFireballAttack;
//			steamManager.secondControllerAttack = steamFireballAttack;


			// Resize the player down to creature-scale
            playerLogic.gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            playerLogic.gameObject.transform.position = new Vector3(0.0f, 2.0f, 0.0f);


			// Slow the player down to human-speed if movement is enabled
            //if (!disableMovement)
            //{
            //    hmdMovement.moveSpeed /= 12;
            //}

			// Change To the offensive abilities
//			offensiveAbilities.rightHandFlipAttack = offensiveAbilities.rightHandOffensiveFlip;
			offensiveAbilities.leftHandFlipAttack = offensiveAbilities.leftHandOffensiveFlip;
			offensiveAbilities.rightHandFistAttack = offensiveAbilities.rightHandOffensiveFist;
			offensiveAbilities.leftHandFistAttack = offensiveAbilities.leftHandOffensiveFist;

            //controllerAttacks.rightTriggerAttack = controllerAttacks.fireballAttack;
            //controllerAttacks.leftTriggerAttack = controllerAttacks.iceballAttack;

			// Start the next round, spawn enemies, wait for the timer
			defensivePhaseMusic.Pause();
			mainGameMusic.Play();
			nextRound = false;
			roundActive = true;
			currentRoundTime = enemyWaves[i].roundTime;
			// Enable the player HUD
//			playerHud.SetActive(true);

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
//			hmdMovement.enabled = false;
			mainGameMusic.Pause();


			offensiveAbilities.deactivateFlameThrowers();
			offensiveAbilities.controlCheck();
		}

		killPlayerEndGame (true);
	}


	// When the player dies bring up the end game screen and stop the current round
	public void killPlayerEndGame(bool win)
	{
		if (!disableMovement)
		{
			hmdMovement.enabled = false;
		}

		mainGameMusic.Pause ();
		defensivePhaseMusic.Pause();
		defensivePhaseMusic.mute = true;
		StopCoroutine (roundFunction ());
        //Destroy (endRoundScreen);
//		endRoundScreen.disableUI();
//		endRoundScreen.enabled = false;
        //endRoundScreen.SetActive (false);
//		playerHud.SetActive (false);

//		offensiveAbilities.rightHandFlipAttack = offensiveAbilities.emptyAttack;
		offensiveAbilities.leftHandFlipAttack = offensiveAbilities.emptyAttack;
		offensiveAbilities.rightHandFistAttack = offensiveAbilities.emptyAttack;
		offensiveAbilities.leftHandFistAttack = offensiveAbilities.emptyAttack;


//		offensiveAbilities.fistAttack = emptyAttack;
//		offensiveAbilities.handFlipAttack = emptyAttack;


		// Now deactivate all active enemies
		for (int enemyIndex = 0; enemyIndex < spawnedEnemies.transform.childCount; enemyIndex++)
		{
			Destroy (spawnedEnemies.transform.GetChild(enemyIndex).gameObject);
		}

		if (win) 
		{
			winningSound.Play();
			endGameText.text = "YOU WIN";
			endGameHud.SetActive (true);
		}
		else
		{
			losingSound.Play();
			endGameHud.SetActive (true);
			// Tell the player how many waves they survived
		}
		if (waveIndex == 1) 
		{
				roundsSurvivedText.text = "You survived for " + waveIndex + " round!";
		} 
		else
		{
				roundsSurvivedText.text = "You survived for " + waveIndex + " rounds!";
		}

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