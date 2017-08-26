using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerLogic : MonoBehaviour, IUnit
{
	private GameLogic game;

    public int startingHealth;

	private int health;

    private bool isAlive = true;

	// Use this for initialization
	void Start () 
	{
        health = startingHealth;
        game = GetObjects.getGame();
	}

	// Update is called once per frame
	void Update ()
	{
	}

	public void respawn()
	{
        if (game != null)
        {
            isAlive = false;
            game.killPlayerEndGame(false);
        }
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

    public bool getIsAlive()
    {
        return isAlive;
    }
}

