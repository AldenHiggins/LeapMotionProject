using UnityEngine;
using System.Collections;

public class MoveFireball : MonoBehaviour {
	private Vector3 velocity;


	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (velocity != null)
		{
			gameObject.transform.position += velocity;
		}
	}

	public void setVelocity(Vector3 newVelocity)
	{
		velocity = newVelocity;
	}


	void OnCollisionEnter (Collision col)
	{
		print ("Got Hit!");
//		if(col.gameObject.name == "Pyroclastic Puff(Clone)")
//		{
//			print ("Found fireball!");
//			//			Destroy(col.gameObject);
//			transform.position = game.RandomPointOnPlane();
//			
//		}
	}
}
