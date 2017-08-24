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
				GameObject monster = (GameObject) Instantiate (spawnThis, transform.position, Quaternion.identity);
				monster.transform.parent = spawnedEnemyList.transform;
				BasicEnemyController enemy = (BasicEnemyController) monster.GetComponent(typeof(BasicEnemyController));
				enemy.enabled = true;
				UnityEngine.AI.NavMeshAgent agent = monster.GetComponent<UnityEngine.AI.NavMeshAgent>();
				agent.enabled = true;
			}
			yield return new WaitForSeconds(spawnFrequency);
		}
	}

	public void spawnEnemy(GameObject enemyToSpawn)
	{
		GameObject monster = (GameObject) Instantiate (enemyToSpawn, transform.position, Quaternion.identity);
		monster.transform.parent = spawnedEnemyList.transform;

		BasicEnemyController enemy = (BasicEnemyController) monster.GetComponent(typeof(BasicEnemyController));

		if (enemy == null)
		{
<<<<<<< HEAD
			enemy = (BasicEnemyController)monster.transform.GetChild (0).GetComponent (typeof(BasicEnemyController));
			UnityEngine.AI.NavMeshAgent agent = monster.transform.GetChild (0).GetComponent<UnityEngine.AI.NavMeshAgent>();
=======
			enemy = (BasicEnemyController) monster.transform.GetChild (0).GetComponent (typeof(BasicEnemyController));
			NavMeshAgent agent = monster.transform.GetChild (0).GetComponent<NavMeshAgent>();
>>>>>>> c5c8868af13028fa52019e2b91ab548f24e18634
			agent.enabled = true;
		}
		else
		{
			UnityEngine.AI.NavMeshAgent agent = monster.GetComponent<UnityEngine.AI.NavMeshAgent>();
			agent.enabled = true;
		}

		enemy.enabled = true;
	}
}
