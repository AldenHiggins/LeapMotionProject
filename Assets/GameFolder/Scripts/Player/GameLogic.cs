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
	public HandController handController = null;
	public int startingPlayerCurrency;
	// PLAYER ATTACKS
	public GameObject fireBall;
	public GameObject clapProjectile;
	// LEVEL ROUNDS
	public int[] rounds;
	public UIFollowPlayer endRoundScreen;
	private int currentRound = 0;
	public GameObject winScreen;
	public Text roundText;
	private bool nextRound = false;
	public ButtonDemoGraphics roundButton;
	private float currentRoundTime = 0;
	private bool roundActive = false;
	public Text roundTimerText;
	public ButtonDemoToggle nextRoundButton;
	// Player HUD
	public GameObject playerHud;
	// ENEMY SPAWNERS
	public GameObject enemySpawners;
	public GameObject spawnedEnemies;
	public GameObject clapFireball;
	// INTERNAL VARIABLES
	private NetworkView view;
	private bool fireballCharged;
	private bool isBlocking;
	private GameObject playerAvatar;
	private int fireballTimer;
	private Networking network;
	private PlayerLogic playerLogic;
	private DefensiveAbilities defensiveAbilities;
	private OffensiveAbilities offensiveAbilities;
	private bool previousSwitchPressed = false;
	// PLAYER CURRENCY
	public Text currencyText;
	private int currentPlayerCurrency;

	// GAME CONTROLLER VARIABLES

	// RUNTIME GAME REPRESENTATION
	private Dictionary<int, GameObject> projectiles;

	// Initialize variables
	void Start () 
	{
		currentPlayerCurrency = startingPlayerCurrency;
		view = gameObject.networkView;
		fireballCharged = false;
		isBlocking = false;
		fireballTimer = 0;
		projectiles = new Dictionary<int, GameObject> ();
		network = (Networking) gameObject.GetComponent (typeof(Networking));
		playerLogic = (PlayerLogic) thisPlayer.GetComponent (typeof(PlayerLogic));
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
		// Start up the round timer
		StartCoroutine (roundFunction());
	}

	// Control loop to check for player input
	void Update () 
	{
		// Check if round is active in order for players to use their abilities
		if (roundActive)
		{
			if (!playerLogic.isDefensivePlayer)
			{
				offensiveAbilities.controlCheck();
			}
			else
			{
				defensiveAbilities.controlCheck();
			}
		}

		// Shared abilities
		bool switchButtonPressed = OVRGamepadController.GPC_GetButton (OVRGamepadController.Button.Back);

		if (!switchButtonPressed && previousSwitchPressed)
		{
			print ("Switching is happening");
			playerLogic.switchOffensiveDefensive ();
		}

		// Update current currency gui value
		currencyText.text = "" + currentPlayerCurrency;

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
		for (int i = 0; i < rounds.GetLength(0); i++)
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

			// Disable hand controller for defensive player
			if (playerLogic.isDefensivePlayer)
			{
				HandController hand = (HandController) thisPlayer.transform.GetChild (1).GetChild (1).
					GetChild (0).gameObject.GetComponent(typeof(HandController));
				hand.DestroyAllHands();
				hand.enabled = false;
			}

			// Start the next round, spawn enemies, wait for the timer
			nextRound = false;
			roundActive = true;
			currentRoundTime = rounds[i];
			endRoundScreen.disableUI();
			enableDisableSpawners(true);
			// Enable the player HUD
			playerHud.SetActive(true);

			// Wait for the round to time out
			yield return new WaitForSeconds(rounds[i]);

			// Turn off spawners
			enableDisableSpawners(false);

			// Wait for all the enemies to be cleared
			while (spawnedEnemies.transform.childCount > 0)
			{
				yield return new WaitForSeconds(.3f);
			}

			roundActive = false;
		}

		// The game/map is over, display end game screen
		winScreen.SetActive(true);
	}

	public void enableDisableSpawners(bool enableOrDisable)
	{
		for (int i = 0; i < enemySpawners.transform.childCount; i++)
		{

			GameObject spawner = enemySpawners.transform.GetChild(i).gameObject;
			EnemySpawner spawnScript = (EnemySpawner) spawner.GetComponent(typeof(EnemySpawner));
			if (enableOrDisable)
			{
				spawnScript.startSpawning();
			}
			else
			{
				spawnScript.stopSpawning();
			}
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
		newFireball.SetActive(true); 
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


	// CURRENCY FUNCTIONALITY
	public int getCurrencyValue()
	{
		return currentPlayerCurrency;
	}

	public void changeCurrency(int currencyChange)
	{
		currentPlayerCurrency += currencyChange;
	}

	public void fistProjectile(){

		// Make sure the fireball spawns in front of the player at a reasonable distance
		Vector3 spawnPosition = thisCamera.transform.position;
		spawnPosition += new Vector3(thisCamera.transform.forward.normalized.x * .8f, thisCamera.transform.forward.normalized.y * .8f, thisCamera.transform.forward.normalized.z * .8f);
		// Scale the fireball's velocity
		Vector3 startingVelocity = thisCamera.transform.forward.normalized;
		startingVelocity *= .2f;

		GameObject newAttack = (GameObject) Instantiate(clapFireball, spawnPosition, Quaternion.identity);
		MoveFireball moveThis = (MoveFireball) newAttack.GetComponent(typeof(MoveFireball));
		moveThis.setVelocity(startingVelocity);
		newAttack.renderer.enabled = true;

			
	}

}
