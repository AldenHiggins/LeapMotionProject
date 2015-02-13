using UnityEngine;
using System.Collections;

public class LevelBoundary : MonoBehaviour 
{
	void OnCollisionEnter(Collision collision)
	{
		print ("Collided with floor");
		Destroy (collision.collider.gameObject);
	}
}
