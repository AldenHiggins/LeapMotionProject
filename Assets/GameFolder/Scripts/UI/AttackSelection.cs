using UnityEngine;
using System.Collections;

public class AttackSelection : MonoBehaviour 
{
	private ButtonDemoToggle[] chosenAttacks;
	private ButtonDemoToggle chosenAttack;

	private ButtonDemoToggle[] attackChoices;
	private ButtonDemoToggle attackChoice;
	private int currentAttackIndex = 0;

	// The in game attacks to be used
	public GameObject inGameAttackBarUI;
	// Offensive abilities where the attacks will be registered
	public OffensiveAbilities offense;

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
			chosenAttacks[i] = (ButtonDemoToggle) transform.GetChild (0).GetChild(i).GetChild(2).gameObject.GetComponent(typeof(ButtonDemoToggle));
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
			print ("Chosen attack was highlighted");
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

				copiedNewAttack.transform.localScale = newAttack.transform.localScale;

				// Find out which attack to change, as of right now:
				// 0 is the hand flip
				// 1 is the fist
				// 2 is the hand circle motion
				// 3 is the clap
				int modifiedAttackIndex = 0;
				for (int i = 0; i < chosenAttacks.Length; i++)
				{
					if (chosenAttacks[i] == pressedAttackButton)
					{
						modifiedAttackIndex = i;
					}
				}

				// Now change the respsective attack in offensive abilities
				if (modifiedAttackIndex == 0)
				{
					offense.uiHandFlipAttack = (AAttack) copiedNewAttack.GetComponent(typeof(AAttack));
				}
				else if (modifiedAttackIndex == 1)
				{
					offense.uiHandFistAttack = (AAttack) copiedNewAttack.GetComponent(typeof(AAttack));
				}

				// Now change the icon in the in game UI
				GameObject oldAttackIcon = inGameAttackBarUI.transform.GetChild(modifiedAttackIndex).GetChild(0).gameObject;
				GameObject newAttackIcon = (GameObject) Instantiate(newAttack);
//				newAttack.transform.parent = inGameAttackBarUI.transform.GetChild(modifiedAttackIndex);
				newAttackIcon.transform.parent = inGameAttackBarUI.transform.GetChild(modifiedAttackIndex);
				newAttackIcon.transform.localPosition = oldAttackIcon.transform.localPosition;

				Destroy (oldAttackIcon);


				Destroy (oldAttack);

				pressedAttackButton.ButtonTurnsOff();
				attackChoice.ButtonTurnsOff();
				attackChoice = null;
			}
		}
		// Attack choice was highlighted
		else
		{
			attackChoice = pressedAttackButton;
			print (transform.GetChild(1).childCount);
			// Iterate through all other attacks and disable them
			for (int i = 0; i < transform.GetChild (1).childCount; i++)
			{
				attackChoices[i] = (ButtonDemoToggle) transform.GetChild (1).GetChild(i).GetChild(2).gameObject.GetComponent(typeof(ButtonDemoToggle));
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
