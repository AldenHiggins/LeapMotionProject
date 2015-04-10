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

		Destroy (gameObject);
	}
}
