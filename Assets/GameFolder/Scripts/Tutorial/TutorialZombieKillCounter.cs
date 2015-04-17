using UnityEngine;
using System.Collections;

public class TutorialZombieKillCounter : MonoBehaviour {

	public GameObject tutorialObject;
	public bool isOffense;
	public bool isDefense;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnParticleCollision(GameObject other)
	{
		if (isOffense)
			tutorialObject.GetComponent<TutorialOffensiveScript> ().killedZombie ();

		StartCoroutine (ZombieDeath ());
	}

	IEnumerator ZombieDeath()
	{
		gameObject.transform.GetChild (0).gameObject.GetComponent<Animator> ().SetBool ("Dead", true);
		gameObject.GetComponent<TutorialEnemyController> ().speed = 0.0f;
		yield return new WaitForSeconds (2.0f);
		Destroy (gameObject);
	}
}
