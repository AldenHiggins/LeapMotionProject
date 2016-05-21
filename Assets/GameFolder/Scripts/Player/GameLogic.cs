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
	// GAME CONTROLLER VARIABLES
	private bool isBlocking;

	// Initialize variables
	void Start () 
	{
		isBlocking = false;
        mainCamera = GetObjects.getCamera();
        playerLogic = GetObjects.getPlayer();
        spawnedEnemies = GetObjects.getSpawnedEnemies();
        goalPosition = GetObjects.getGoalPosition();
        waveContainer = GetObjects.getEnemyWaves();

		// Initialize waves
		enemyWaves = new MonsterWave[waveContainer.transform.childCount];
		for (int i = 0; i < waveContainer.transform.childCount; i++)
		{
			enemyWaves[i] = (MonsterWave) waveContainer.transform.GetChild(i).gameObject.GetComponent(typeof(MonsterWave));
		}
		// Start up the round timer
		StartCoroutine (roundFunction());
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
            
			// Wait for player to end defensive setup phase
			while (!roundActive) 
			{
				yield return new WaitForSeconds(.2f);
			}

            ///////////////////////////////////////
            ///////    OFFENSIVE PHASE   //////////
            ///////////////////////////////////////

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
		}

		killPlayerEndGame (true);
	}

    public void startRound()
    {
        roundActive = true;
    }

    // When the player dies bring up the end game screen and stop the current round
    public void killPlayerEndGame(bool win)
	{
		StopCoroutine (roundFunction ());

        Debug.Log("Game over!!");

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
}
