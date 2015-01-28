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
			Instantiate (spawnThis, transform.position, Quaternion.identity);
			yield return new WaitForSeconds(spawnFrequency);
		}
	}
}

