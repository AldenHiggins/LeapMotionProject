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

	// Contains the ability choices
	public GameObject abilityChoices;

	// Selections that determine what type of attack we are modifying
	public GameObject offenseDefense;
	// 0 is offense, 1 is defense
	private int offenseOrDefense = 0;
	public GameObject leftRight;
	// 0 is right, 1 is left
	private int leftOrRight = 0;
	// 0 is flip, 1 is fist
	public GameObject flipFist;
	private int flipOrFist = 0;

	// Use this for initialization
	void Start (){}



	// Update is called once per frame
	void Update ()
	{
		// Check the various selection parameters to see which ones are applicable
		ButtonDemoToggle button = (ButtonDemoToggle)offenseDefense.transform.GetChild (0).GetComponentInChildren (typeof(ButtonDemoToggle));
		if (button.onGraphics.isPressed())
		{
			offenseOrDefense = 0;
			// Modify the available ability choices based on whether offense or defense was chosen
			abilityChoices.transform.GetChild(0).gameObject.SetActive(true);
			abilityChoices.transform.GetChild(1).gameObject.SetActive(false);

		}
		else
		{
			offenseOrDefense = 1;
			// Modify the available ability choices based on whether offense or defense was chosen
			abilityChoices.transform.GetChild(0).gameObject.SetActive(false);
			abilityChoices.transform.GetChild(1).gameObject.SetActive(true);
		}

		button = (ButtonDemoToggle)leftRight.transform.GetChild (0).GetComponentInChildren (typeof(ButtonDemoToggle));
		if (button.onGraphics.isPressed())
		{
			leftOrRight = 0;
		}
		else
		{
			leftOrRight = 1;
		}

		button = (ButtonDemoToggle)flipFist.transform.GetChild (0).GetComponentInChildren (typeof(ButtonDemoToggle));
		if (button.onGraphics.isPressed())
		{
			flipOrFist = 0;
		}
		else
		{
			flipOrFist = 1;
		}
	}


	public void newAttackPressed(AAttack newAttackChoice)
	{
		//Defense chosen
		if (offenseOrDefense == 1)
		{
			//left chosen
			if (leftOrRight == 1)
			{
				//fist chosen
				if (flipOrFist == 1)
				{
					offense.leftHandDefensiveFist = newAttackChoice;
				}
				//flip
				else
				{
					offense.leftHandDefensiveFlip = newAttackChoice;
				}
			}
			// right chosen
			else
			{
				//fist chosen
				if (flipOrFist == 1)
				{
					offense.rightHandDefensiveFist = newAttackChoice;
				}
				//flip
				else
				{
					offense.rightHandDefensiveFlip = newAttackChoice;
				}
			}

		}
		//Offense chosen
		else
		{
			//left chosen
			if (leftOrRight == 1)
			{
				//fist chosen
				if (flipOrFist == 1)
				{
					offense.leftHandOffensiveFist = newAttackChoice;
				}
				//flip
				else
				{
					offense.leftHandOffensiveFlip = newAttackChoice;
				}
			}
			// right chosen
			else
			{
				//fist chosen
				if (flipOrFist == 1)
				{
					offense.rightHandOffensiveFist = newAttackChoice;
				}
				//flip
				else
				{
					offense.rightHandOffensiveFlip = newAttackChoice;
				}
			}
		}
	}

	public void highlightAttackChoice(ButtonDemoToggle pressedAttackButton){}
}
