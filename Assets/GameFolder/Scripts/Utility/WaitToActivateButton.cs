using UnityEngine;
using System.Collections;

public class WaitToActivateButton : MonoBehaviour {

	public ButtonDemoToggle buttonToActivate;

	// Use this for initialization
	void Start () {
		StartCoroutine (waitBeforeActivation());
	}

	IEnumerator waitBeforeActivation()
	{
		yield return new WaitForSeconds (3f);
		buttonToActivate.enabled = true;
	}
}
