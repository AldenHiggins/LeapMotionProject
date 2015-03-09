using UnityEngine;
using System.Collections;

public class MonsterWave : MonoBehaviour 
{
	public float roundTime;
	public GameObject[] enemiesToSpawn;
	public int[] spawnAmounts;
	public EnemySpawner[] spawners;

	private int currentSpawnerIndex = 0;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void startWave()
	{
		for (int i = 0; i < enemiesToSpawn.Length; i++)
		{
			StartCoroutine(spawnEnemyType(i));
		}
	}

	IEnumerator spawnEnemyType(int enemyIndex)
	{
		for (int i = 0; i < spawnAmounts[enemyIndex]; i++)
		{
			// Cycle through the available spawners
			currentSpawnerIndex++;
			if (currentSpawnerIndex > spawners.Length - 1)
				currentSpawnerIndex = 0;

			spawners[currentSpawnerIndex].spawnEnemy(enemiesToSpawn[enemyIndex]);

			print ("Waiting this long to spawn next enemy: " + roundTime/spawnAmounts[enemyIndex]);
			yield return new WaitForSeconds(roundTime/spawnAmounts[enemyIndex]);
		}


	}
}
