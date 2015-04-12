using UnityEngine;
using System.Collections;

public class TutorialOneHitZombie : MonoBehaviour
{
	public GameObject tutorialObject;
	public bool handFlipZombie;
	public bool handFistZombie;


	void OnParticleCollision(GameObject other)
	{
		if (handFlipZombie) {
			print("Hand Flip Zombie Killed");
			tutorialObject.GetComponent<TutorialOffensiveScript> ().handFlipZombieKilled ();
		}

		if (handFistZombie)
			tutorialObject.GetComponent<TutorialOffensiveScript> ().handFistZombieKilled ();

		Destroy (gameObject);
	}
}
