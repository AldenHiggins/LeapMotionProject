using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialDefensiveCollisionNotifier : MonoBehaviour {

	public GameObject tutorialObject;
	public bool isBallista;
	public bool isOilSlick;
	
	
	void OnCollisionEnter(Collision collision)
	{
		if (isBallista) {
			print("Ballista Bolt Collided");
			tutorialObject.GetComponent<TutorialDefensiveScript> ().ballistaKilledZombie();

			Destroy(gameObject);
		}
	
	}

	void OnParticleCollision(GameObject other)
	{
		if (isOilSlick) {
			print("Oil Slick Exploded - Particle Collsion Detected");
			tutorialObject.GetComponent<TutorialDefensiveScript> ().oilSlickExploded();
		}
	}
	

	void OnTriggerEnter(Collider other) 
	{

		if (isOilSlick) {
			print ("Oil Slick Stopped Zombie");
			tutorialObject.GetComponent<TutorialDefensiveScript> ().oilSlickStoppedZombie ();
		}

	}

}
