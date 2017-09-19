using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour 
{
	public GameObject spawnThis;
	public GameObject spawnedEnemyList;

	public float spawnFrequency;

	private bool spawning = true;
	
	// Use this for initialization
	void Start () 
	{
        StartCoroutine(spawnLoop());
    }

    // Update is called once per frame
    void Update () 
	{	
	}

	public void stopSpawning()
	{
		spawning = false;
	}

	public void startSpawning()
	{
		spawning = true;
		StartCoroutine (spawnLoop ());

	}

	IEnumerator spawnLoop()
	{
		while(true)
		{
			if (spawning)
			{
				GameObject monster = Instantiate (spawnThis, transform.position, Quaternion.identity);
				monster.transform.parent = spawnedEnemyList.transform;
			}
			yield return new WaitForSeconds(spawnFrequency);
		}
	}

    public void spawnEnemy(GameObject enemyToSpawn)
    {
        GameObject monster = Instantiate(enemyToSpawn, transform.position, Quaternion.identity);
        monster.transform.parent = spawnedEnemyList.transform;
    }
}
