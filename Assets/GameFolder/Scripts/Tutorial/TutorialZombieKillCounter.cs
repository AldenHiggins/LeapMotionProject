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
		//if (isOffense)
			//tutorialObject.GetComponent<TutorialOffensiveScript> ().killedZombie ();

		StartCoroutine (ZombieDeath ());
	}

	IEnumerator ZombieDeath()
	{
		gameObject.GetComponent<Animator> ().SetBool ("Dead", true);
		gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().enabled = false;
		yield return new WaitForSeconds (4.0f);
		Destroy (gameObject);
	}
}
