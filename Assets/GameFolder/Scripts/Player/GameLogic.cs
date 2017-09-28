using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameLogic : MonoBehaviour 
{
	// CAMERA
	private GameObject mainCamera;
	// ENEMY SPAWNERS
	private GameObject spawnedEnemies;
	// LEVEL ROUNDS
	public bool roundActive = false;
	// MONSTER WAVES
	private MonsterWave[] enemyWaves;
	// INTERNAL VARIABLES
	private PlayerLogic playerLogic;
    // STILL ON START SCREEN
    private bool startScreenActive = true;

	// Initialize variables
	void Start () 
	{
        mainCamera = GetObjects.instance.getCamera();
        playerLogic = GetObjects.instance.getPlayer();
        spawnedEnemies = GetObjects.instance.getSpawnedEnemies().gameObject;

        // Install event listeners
        EventManager.StartListening(GameEvents.GameStart, startGame);
    }

	IEnumerator roundFunction()
	{
		for (int i = 0; i < enemyWaves.Length; i++)
		{
            ///////////////////////////////////////
            /////////    RESET ROUND   ////////////
            ///////////////////////////////////////
            playerLogic.resetHealth();

            ///////////////////////////////////////
            ///////    DEFENSIVE PHASE   //////////
            ///////////////////////////////////////
            EventManager.TriggerEvent(GameEvents.DefensivePhaseStart);

            // Wait for player to end defensive setup phase
            while (!roundActive) 
			{
				yield return new WaitForSeconds(.2f);
			}

            ///////////////////////////////////////
            ///////    OFFENSIVE PHASE   //////////
            ///////////////////////////////////////
            EventManager.TriggerEvent(GameEvents.OffensivePhaseStart);

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
		}

		killPlayerEndGame (true);
	}

    public void startRound()
    {
        roundActive = true;
    }

    // When the player dies stop the current round
    public void killPlayerEndGame(bool win)
	{
		StopCoroutine (roundFunction ());

        Debug.Log("Game over!!");
        EventManager.TriggerEvent(GameEvents.GameOver);

		// Now deactivate all active enemies
		for (int enemyIndex = 0; enemyIndex < spawnedEnemies.transform.childCount; enemyIndex++)
		{
			Destroy (spawnedEnemies.transform.GetChild(enemyIndex).gameObject);
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

    public void startGame()
    {
        startScreenActive = false;

        // Start up the round timer
        StartCoroutine(roundFunction());
    }

    public bool getStartScreenActive()
    {
        return startScreenActive;
    }
}
