using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialSpawning : MonoBehaviour 
{
	public GameObject spawnThis;
	public GameObject spawnedEnemyList;
	public GameObject target;
	
	public float spawnFrequency;
	
	private bool spawning = true;

	private int numberSpawned = 0;
	// Use this for initialization
	void Start () 
	{
//		StartCoroutine (spawnLoop ());
	}
	
	// Update is called once per frame
	void Update () 
	{	
	}
	
	public void stopSpawning()
	{
		spawning = false;
		StopCoroutine (spawnLoop ());
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

				monster.transform.GetChild(0).transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
				BasicEnemyController enemy = (BasicEnemyController) monster.GetComponentInChildren(typeof(BasicEnemyController));
				enemy.enabled = true;
				UnityEngine.AI.NavMeshAgent agent = monster.GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
				agent.enabled = true;
				agent.SetDestination(target.transform.position);
			}
			yield return new WaitForSeconds(spawnFrequency);
		}
	}
	
	public void spawnEnemy(GameObject enemyToSpawn)
	{
		GameObject monster = (GameObject) Instantiate (enemyToSpawn, transform.position, Quaternion.identity);
		monster.transform.parent = spawnedEnemyList.transform;
		BasicEnemyController enemy = (BasicEnemyController) monster.GetComponent(typeof(BasicEnemyController));
		enemy.enabled = true;
		UnityEngine.AI.NavMeshAgent agent = monster.GetComponent<UnityEngine.AI.NavMeshAgent>();
		agent.enabled = true;
	}
	
	
}

