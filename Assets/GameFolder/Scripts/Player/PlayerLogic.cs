using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerLogic : MonoBehaviour, IUnit
{
	private GameLogic game;

	private int health;

	// Use this for initialization
	void Start () 
	{
        game = GetObjects.getGame();
	}

	// Update is called once per frame
	void Update ()
	{
	}

	public void respawn()
	{
		game.killPlayerEndGame (false);
	}

	public void dealDamage(int damageToDeal)
	{
		health -= damageToDeal;

		// Player is dead
		if (health < 0)
		{
			respawn();
		}
	}

	public int getHealth()
	{
		return health;
	}

	public void resetHealth()
	{
		health = 100;
	}

	public GameObject getGameObject()
	{
		return gameObject;
	}
}

