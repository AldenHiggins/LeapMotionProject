using UnityEngine;
using System.Collections;

public class TutorialOneHitZombie : MonoBehaviour
{
	public GameObject oldMessage;
	public GameObject newMessage;


//	void OnCollisionEnter(Collision coll)
//	{
//		print ("On this end");
//
//		print (coll.collider.name);
//		if (coll.collider.name == "Roof")
//		{
//			return;
//		}
//
//	}

	void OnParticleCollision(GameObject other)
	{
		oldMessage.SetActive (false);
		newMessage.SetActive (true);
		
		Destroy (gameObject);
	}
}
