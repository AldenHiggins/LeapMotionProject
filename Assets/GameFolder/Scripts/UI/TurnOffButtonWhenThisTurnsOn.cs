using System.Collections;
using UnityEngine;


public class TurnOffButtonWhenThisTurnsOn : MonoBehaviour
{
	private ButtonDemoToggle thisButton;
	public ButtonDemoToggle[] buttonToTurnOff;
	private bool otherButtonFlipped = false;
	public bool startSelected;

	void Start()
	{
		thisButton = (ButtonDemoToggle) gameObject.GetComponent (typeof(ButtonDemoToggle));
		if (startSelected)
		{
			StartCoroutine(delayedStartup());
		}
	}

	IEnumerator delayedStartup()
	{
		yield return new WaitForSeconds(1.0f);
		thisButton.ButtonTurnsOn ();
	}

	void Update()
	{
		if (thisButton.onGraphics.gameObject.GetComponentInChildren<Renderer>().enabled)
		{
			if (!otherButtonFlipped)
			{
				otherButtonFlipped = true;

				for (int buttonIndex = 0; buttonIndex < buttonToTurnOff.Length; buttonIndex++)
				{
					buttonToTurnOff[buttonIndex].enabled = true;
					buttonToTurnOff[buttonIndex].ButtonTurnsOff();
				}
//				buttonToTurnOff.enabled = true;
//				buttonToTurnOff.ButtonTurnsOff();
				// Disable collider to prevent a person from disabling this option by pressing the button
				gameObject.GetComponent<BoxCollider>().enabled = false;
				ButtonDemoToggle toggle = (ButtonDemoToggle) gameObject.GetComponent(typeof(ButtonDemoToggle));
				toggle.enabled = false;
			}
		}
		else
		{
			otherButtonFlipped = false;
			gameObject.GetComponent<BoxCollider>().enabled = true;
		}
	}
}

