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
}
