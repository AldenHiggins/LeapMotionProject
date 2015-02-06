using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour 
{
	public GameObject spawnThis;
	public float spawnFrequency;
	
	// Use this for initialization
	void Start () 
	{
		StartCoroutine (spawnLoop ());
	}
	
	// Update is called once per frame
	void Update () 
	{	
	}

	
	IEnumerator spawnLoop()
	{
		while(true)
		{
			GameObject monster = (GameObject) Instantiate (spawnThis, transform.position, Quaternion.identity);
			BasicEnemyController enemy = (BasicEnemyController) monster.GetComponent(typeof(BasicEnemyController));
			enemy.enabled = true;
			yield return new WaitForSeconds(spawnFrequency);
		}
	}
}

