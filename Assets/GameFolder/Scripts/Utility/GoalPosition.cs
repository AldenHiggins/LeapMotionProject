using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GoalPosition : MonoBehaviour 
{
	public GameObject monsterReachGoalParticle;
	public GameObject playerHud;
	public GameObject gameOver;
	public Text livesText;
	public int playerLives;

	private int startingLives;

	// Use this for initialization
	void Start ()
	{
		startingLives = playerLives;
		livesText.text = "Lives: " + playerLives + " of " + playerLives;
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
			// Update the life counter
			else
			{
				livesText.text = "Lives: " + playerLives + " of " + startingLives;
			}
		}
	}
}
