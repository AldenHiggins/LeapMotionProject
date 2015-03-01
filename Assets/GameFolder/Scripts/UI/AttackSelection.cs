using UnityEngine;
using System.Collections;

public class AttackSelection : MonoBehaviour 
{
	private ButtonDemoToggle[] chosenAttacks;
	private ButtonDemoToggle chosenAttack;

	private ButtonDemoToggle[] attackChoices;
	private ButtonDemoToggle attackChoice;
	private int currentAttackIndex = 0;

	// Use this for initialization
	void Start () 
	{
		chosenAttack = null;
		// Find all of the chosen attacks
		Transform attacksParent = transform.GetChild (0);
		chosenAttacks = new ButtonDemoToggle[attacksParent.childCount];
		// Initialize chosen attacks
		for (int i = 0; i < attacksParent.childCount; i++)
		{
			chosenAttacks[i] = (ButtonDemoToggle) attacksParent.GetChild(i).GetChild(1).gameObject.GetComponent(typeof(ButtonDemoToggle));
		}

		// Initialize attack choices
		attacksParent = transform.GetChild (1);
		attackChoices = new ButtonDemoToggle[attacksParent.childCount];
		for (int i = 0; i < attacksParent.childCount; i++)
		{
			attackChoices[i] = (ButtonDemoToggle) attacksParent.GetChild(i).GetChild(1).gameObject.GetComponent(typeof(ButtonDemoToggle));
		}
	}
	
	// Update is called once per frame
	void Update ()
	{

	}

	bool isButtonChosenAttack(ButtonDemoToggle attackToCheck)
	{
		// Check if the given button is a chosen attack
		for (int i = 0; i < transform.GetChild (0).childCount; i++)
		{
			chosenAttacks[i] = (ButtonDemoToggle) transform.GetChild (0).GetChild(i).GetChild(1).gameObject.GetComponent(typeof(ButtonDemoToggle));
			if (attackToCheck == chosenAttacks[i])
			{
				return true;
			}
		}
		return false;
	}

	public void highlightAttackChoice(ButtonDemoToggle pressedAttackButton)
	{
		// Chosen attack was highlighted
		if (isButtonChosenAttack(pressedAttackButton))
		{
			if (attackChoice == null)
			{
				pressedAttackButton.ButtonTurnsOff();
			}
			else
			{
				GameObject oldAttack = pressedAttackButton.transform.parent.GetChild(0).gameObject;
				
				GameObject newAttack = attackChoice.transform.parent.GetChild (0).gameObject;
				
				GameObject copiedNewAttack = (GameObject) Instantiate(newAttack);
				
				copiedNewAttack.transform.parent = oldAttack.transform.parent;

				copiedNewAttack.transform.localPosition = oldAttack.transform.localPosition;

				copiedNewAttack.transform.localScale = oldAttack.transform.localScale;
				
				Destroy (oldAttack);

				pressedAttackButton.ButtonTurnsOff();
				attackChoice.ButtonTurnsOff();
				attackChoice = null;
			}



//			chosenAttack = pressedAttackButton;
//			print ("Chosen attack!");
//			// Iterate through all other attacks and disable them
//			for (int i = 0; i < transform.GetChild (0).childCount; i++)
//			{
//				chosenAttacks[i] = (ButtonDemoToggle) transform.GetChild (0).GetChild(i).GetChild(1).gameObject.GetComponent(typeof(ButtonDemoToggle));
//				if (chosenAttack == chosenAttacks[i])
//				{
//					print ("Found the right one: " + i);
//				}
//				else
//				{
//					chosenAttacks[i].ButtonTurnsOff();
//				}
//			}
		}
		// Attack choice was highlighted
		else
		{
			attackChoice = pressedAttackButton;
			print ("Attack choice!");
			// Iterate through all other attacks and disable them
			for (int i = 0; i < transform.GetChild (1).childCount; i++)
			{
				attackChoices[i] = (ButtonDemoToggle) transform.GetChild (1).GetChild(i).GetChild(1).gameObject.GetComponent(typeof(ButtonDemoToggle));
				if (attackChoice == attackChoices[i])
				{
					print ("Found the right one: " + i);
				}
				else
				{
					attackChoices[i].ButtonTurnsOff();
				}
			}
		}
	}

	public void deselectAttack(ButtonDemoToggle turnedOffButton)
	{
		if (chosenAttack == turnedOffButton)
		{
			chosenAttack = null;
		}

		if (attackChoice == turnedOffButton)
		{
			attackChoice = null;
		}
	}



	public int selectNextAttack()
	{
		return currentAttackIndex;
	}


}
