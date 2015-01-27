using UnityEngine;
using System.Collections;

public class PlayerLogic : MonoBehaviour 
{
	public GameLogic game;


	// Use this for initialization
	void Start () 
	{
		print ("This is running!");
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}



	void OnCollisionEnter (Collision col)
	{
		print ("Got Hit!");
		if(col.gameObject.name == "Pyroclastic Puff(Clone)")
		{
			print ("Found fireball!");
			//			Destroy(col.gameObject);
			if (!game.isPlayerBlocking())
			{
				transform.position = game.RandomPointOnPlane();
			}
			else
			{
				print ("Blocked fireball");
				Destroy (col.gameObject);
			}
		}
	}

}
