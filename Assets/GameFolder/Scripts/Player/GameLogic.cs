using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameLogic : MonoBehaviour 
{
	// CAMERA
	private GameObject mainCamera;
    // GOAL POSITION
	private GameObject goalPosition;
	// ENEMY SPAWNERS
	private GameObject spawnedEnemies;
	// LEVEL ROUNDS
	private bool nextRound = false;
	public bool roundActive = false;
	// MONSTER WAVES
	private GameObject waveContainer;
	private MonsterWave[] enemyWaves;
	// INTERNAL VARIABLES
	private PlayerLogic playerLogic;
	private DefensiveAbilities defensiveAbilities;
	private OffensiveAbilities offensiveAbilities;
	// GAME CONTROLLER VARIABLES
	private bool isBlocking;
	// DEFENSIVE STAGE
	private bool isDefensiveStageActive = false;
	// STEAM ACTIVATE OFFENSE
	public bool startRound1;
	public bool startRound2;

	// Initialize variables
	void Start () 
	{
		Debug.Log ("Starting game logic");

		isBlocking = false;
        mainCamera = GetObjects.getCamera();
        playerLogic = GetObjects.getPlayer();
        spawnedEnemies = GetObjects.getSpawnedEnemies();
        goalPosition = GetObjects.getGoalPosition();
        waveContainer = GetObjects.getEnemyWaves();

		Debug.Log ("Initializing offensive abilities");

		offensiveAbilities = (OffensiveAbilities) gameObject.GetComponent (typeof(OffensiveAbilities));
		defensiveAbilities = (DefensiveAbilities) gameObject.GetComponent (typeof(DefensiveAbilities));
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

        developerControlCheck();
	}

	public void startRound()
	{
        Debug.Log("Starting round");
		if (isDefensiveStageActive)
		{
			startRound1 = true;
			startRound2 = true;
		}
		else
		{
			nextRound = true;
		}
	}

	IEnumerator roundFunction()
	{
		for (int i = 0; i < enemyWaves.Length; i++)
		{
            ///////////////////////////////////////
            /////////    RESET ROUND   ////////////
            ///////////////////////////////////////
            playerLogic.resetHealth();

			// Size the player up to giant-scale
			//playerLogic.gameObject.transform.localScale= new Vector3(1.1f, 1.1f, 1.1f);
			//playerLogic.gameObject.transform.position = new Vector3(0.0f, 27.0f, 0.0f);

			// Now wait for the player to press next round button
			while (!nextRound)
			{
				yield return new WaitForSeconds(.2f);
			}

            ///////////////////////////////////////
            ///////    DEFENSIVE PHASE   //////////
            ///////////////////////////////////////

            // Start defensive setup phase
            isDefensiveStageActive = true;

			// Wait a couple of seconds for the player to readjust.
			yield return new WaitForSeconds(1.5f);
            
			// Wait for player to end defensive setup phase
			while (!startRound1 || !startRound2) 
			{
				yield return new WaitForSeconds(.2f);
			}

            ///////////////////////////////////////
            ///////    OFFENSIVE PHASE   //////////
            ///////////////////////////////////////
            startRound1 = false;
			startRound2 = false;

			// Clean up defensive setup stuff
			isDefensiveStageActive = false;

			// Resize the player down to creature-scale
            //playerLogic.gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            //playerLogic.gameObject.transform.position = new Vector3(0.0f, 2.0f, 0.0f);

			// Start the next round, spawn enemies, wait for the timer
			nextRound = false;
			roundActive = true;

			// Start the enemy spawners
			enemyWaves[i].startWave ();
			// Wait for the round to time out
			yield return new WaitForSeconds(enemyWaves[i].roundTime);

			// Wait for all the enemies to be cleared
			while (spawnedEnemies.transform.childCount > 0)
			{
				yield return new WaitForSeconds(.3f);
			}

            ///////////////////////////////////////
            ///////      FINISH ROUND    //////////
            ///////////////////////////////////////
            roundActive = false;
			playerLogic.changeCurrency(500);

			//offensiveAbilities.deactivateFlameThrowers();
			offensiveAbilities.controlCheck();
		}

		killPlayerEndGame (true);
	}


	// When the player dies bring up the end game screen and stop the current round
	public void killPlayerEndGame(bool win)
	{
		StopCoroutine (roundFunction ());
        //Destroy (endRoundScreen);
//		endRoundScreen.disableUI();
//		endRoundScreen.enabled = false;
        //endRoundScreen.SetActive (false);

//		offensiveAbilities.fistAttack = emptyAttack;
//		offensiveAbilities.handFlipAttack = emptyAttack;

		// Now deactivate all active enemies
		for (int enemyIndex = 0; enemyIndex < spawnedEnemies.transform.childCount; enemyIndex++)
		{
			Destroy (spawnedEnemies.transform.GetChild(enemyIndex).gameObject);
		}
	}

    void developerControlCheck()
    {
        // Check for developer/debug actions
        if (Input.GetKeyDown(KeyCode.V))
        {
            startRound();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            startRound1 = true;
            startRound2 = true;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameObject wallToDestroy = GameObject.Find("DefensiveWall(Clone)");
            Destroy(wallToDestroy);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            for (int enemyIndex = 0; enemyIndex < spawnedEnemies.transform.childCount; enemyIndex++)
            {
                Destroy(spawnedEnemies.transform.GetChild(enemyIndex).gameObject);
            }
        }
    }

    public RaycastHit getRayHit()
	{
		int maskOne = 1 << 10;
		int maskTwo = 1 << 11;
		int mask = maskOne | maskTwo;
		Ray ray = new Ray (mainCamera.transform.position, mainCamera.transform.forward);
		RaycastHit hit;
		Physics.Raycast (ray, out hit, 100f, mask);
		return hit;
	}
}
