using UnityEngine;
using System.Collections;

public class PlayerLogic : MonoBehaviour 
{
	public GameLogic game;
	public bool isDefensivePlayer;
	public AudioClip grunt;
	private int health;

	// Use this for initialization
	void Start () 
	{
		if (isDefensivePlayer) {
			transform.position = new Vector3 (55f, 130f, 0f);
			//transform.localScale += Vector3(0.1,0,0);
		}
			
		health = 100;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void respawn()
	{
		transform.position = game.RandomPointOnPlane();
	}

	public void dealDamage(int damageToDeal)
	{
		health -= damageToDeal;
		print ("Taking damage!");
		// Player is dead
		if (health < 0)
		{
			respawn ();
		}
		// Player just takes damage
		else
		{
			AudioSource.PlayClipAtPoint(grunt,transform.position);
		}
	}
}
