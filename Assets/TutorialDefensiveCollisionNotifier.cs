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
			print("Hand Flip Zombie Killed");
			tutorialObject.GetComponent<TutorialDefensiveScript> ().ballistaKilledZombie();

			Destroy(gameObject);
		}
	
	}

	void OnTriggerEnter(Collider other) 
	{

		if (isOilSlick) {

			print ("Hand Fist Zombie Killed");
			tutorialObject.GetComponent<TutorialDefensiveScript> ().oilSlickStoppedZombie ();

		}
		
		Destroy (gameObject);

	}

}
