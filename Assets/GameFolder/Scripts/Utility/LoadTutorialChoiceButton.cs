using UnityEngine;
using System.Collections;

public class LoadTutorialChoiceButton : MonoBehaviour
{

	public GameObject tutorialStart;
	public GameObject firstMessage;
	public ButtonDemoGraphics button;
	public GameObject otherButton;
	

	// Update is called once per frame
	void Update () 
	{
		if (button.isPressed ()) {
			otherButton.SetActive (false);
			gameObject.SetActive (false);
			tutorialStart.GetComponent<TutorialWelcomeMessage>().setNextMessage(firstMessage);
			tutorialStart.SetActive (true);
		}
	}
}
