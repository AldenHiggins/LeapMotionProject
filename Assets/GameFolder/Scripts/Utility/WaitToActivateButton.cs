using UnityEngine;
using System.Collections;

public class WaitToActivateButton : MonoBehaviour {

	public ButtonDemoToggle buttonToActivate;
	public float timeToWait;
	// Use this for initialization
	void Start () {
		StartCoroutine (waitBeforeActivation());
	}

	IEnumerator waitBeforeActivation()
	{
		yield return new WaitForSeconds (timeToWait);
		buttonToActivate.enabled = true;
	}
}
