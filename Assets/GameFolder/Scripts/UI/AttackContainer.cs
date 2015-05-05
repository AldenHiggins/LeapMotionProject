using UnityEngine;
using System.Collections;


public class AttackContainer : MonoBehaviour
{
	public AAttack thisAttack;
	public AttackSelection attackLogic;
	private ButtonDemoToggle thisButton;
	private bool messageSent = false;

	void Start()
	{
		thisButton = (ButtonDemoToggle) gameObject.GetComponentInChildren (typeof(ButtonDemoToggle));
	}

	void Update()
	{
		if (thisButton.onGraphics.isPressed())
		{
			if (!messageSent)
			{
				messageSent = true;
				attackLogic.newAttackPressed(thisAttack);
			}
		}
		// Allow another message to be sent once this button is no longer pressed
		else
		{
			messageSent = false;
		}
	}

}


