using UnityEngine;
using System.Collections;

public class AttackSelection : MonoBehaviour 
{
	private ButtonDemoToggle[] attacks;
	private int currentAttackIndex = 0;

	// Use this for initialization
	void Start () 
	{
		attacks = new ButtonDemoToggle[transform.childCount];
		for (int i = 0; i < transform.childCount; i++)
		{
			attacks[i] = (ButtonDemoToggle) transform.GetChild (i).GetChild (0).gameObject.GetComponent(typeof(ButtonDemoToggle));
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Highlight the currently selected attack
		attacks [currentAttackIndex].ButtonTurnsOn ();

	}

	public int selectNextAttack()
	{
		attacks [currentAttackIndex].ButtonTurnsOff ();
		currentAttackIndex++;
		if (currentAttackIndex >= attacks.Length)
		{
			currentAttackIndex = 0;
		}
		return currentAttackIndex;
	}
}
