using UnityEngine;
using System.Collections;

public class BasicEnemyController : MonoBehaviour {
	public GameObject player;

	private Vector3 velocity;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		velocity = player.transform.position - transform.position;
		velocity.y = 0.0f;
		if (velocity.magnitude > 2)
		{
			velocity.Normalize ();
			velocity *= .02f;
			transform.position += velocity;
		}
	}
}
