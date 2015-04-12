using UnityEngine;
using System.Collections;

public class LoadTutorialChoiceButton : MonoBehaviour
{
	
	public GameObject firstMessage;
	public ButtonDemoGraphics button;
	public GameObject otherButton;
	

	// Update is called once per frame
	void Update () 
	{
		if (button.isPressed ()) {
			otherButton.SetActive (false);
			gameObject.SetActive (false);
			firstMessage.SetActive(true);
		}
	}
}
