using UnityEngine;
using System.Collections;

public class LoadTutorialChoiceButton : MonoBehaviour
{
	public ButtonDemoToggle buttonToggle;
	public GameObject firstMessage;
	public ButtonDemoGraphics button;
	public GameObject otherButton;
	public bool isOffensive;
	public bool isDefensive;
	

	// Update is called once per frame
	void Update () 
	{
		if (button.isPressed ()) {
			otherButton.SetActive (false);
			buttonToggle.ButtonTurnsOff();
			gameObject.SetActive (false);
			firstMessage.SetActive(true);
			firstMessage.SendMessage("activateTutorial");
		}
	}
}
