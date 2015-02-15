using UnityEngine;
using System.Collections;

public class GoalPosition : MonoBehaviour 
{
	public GameObject monsterReachGoalParticle;
	public GameObject playerHud;
	public GameObject gameOver;
	public int playerLives;


	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnTriggerEnter(Collider collider)
	{
		print ("Collision");
		BasicEnemyController enemy = (BasicEnemyController) collider.gameObject.GetComponent(typeof(BasicEnemyController));
		if (enemy != null)
		{
			playerLives -= enemy.livesTakenOnGoalReached;
			Instantiate (monsterReachGoalParticle, enemy.transform.position, Quaternion.Euler(-90.0f, 0.0f, 0.0f));
			Destroy (enemy.gameObject);

			if (playerLives <= 0)
			{
				playerHud.SetActive(false);
				gameOver.SetActive(true);
			}
		}
	}
}
