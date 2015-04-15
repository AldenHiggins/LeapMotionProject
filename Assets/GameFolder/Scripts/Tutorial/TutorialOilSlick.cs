using UnityEngine;
using System.Collections;

public class TutorialOilSlick : MonoBehaviour {
	

	public GameObject tutorialObject;
	
	// Use this for initialization
	void Start () {
	}
	

	public void hasBlownUp() {
		tutorialObject.GetComponent<TutorialDefensiveScript> ().oilSlickExploded ();
	}
}
