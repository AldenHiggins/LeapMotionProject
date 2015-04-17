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

		StartCoroutine (ZombieDeath ());
	}

	IEnumerator ZombieDeath()
	{
		gameObject.transform.GetChild (0).gameObject.GetComponent<Animator> ().SetBool ("Dead", true);
		yield return new WaitForSeconds (2.0f);
		gameObject.transform.GetChild (0).gameObject.GetComponent<Animator> ().SetBool ("Dead", false);
		gameObject.SetActive (false);
	}
}
