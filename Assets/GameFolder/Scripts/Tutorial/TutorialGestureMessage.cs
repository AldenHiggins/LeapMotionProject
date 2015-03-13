using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialGestureMessage : MonoBehaviour 
{
	public GameObject gesture;
	public HandController handController;
	public Text displayMessage;

	public float firstMessageTime;
	public string secondMessage;

	public float secondMessageTime;
	public string thirdMessage;

	public OffensiveAbilities offense;


	// Use this for initialization
	void Start () 
	{
		gesture.SetActive (true);
		StartCoroutine (messageFunction ());
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	IEnumerator messageFunction()
	{
		yield return new WaitForSeconds (firstMessageTime);
		displayMessage.text = secondMessage.Replace("lineline","\n");
		yield return new WaitForSeconds (secondMessageTime);
		displayMessage.text = thirdMessage.Replace("lineline","\n");
		StartCoroutine (activateOffensiveAbilities ());
	}

	IEnumerator activateOffensiveAbilities()
	{
		while(true)
		{
			yield return new WaitForSeconds(.1f);
			offense.controlCheck();
		}
	}
}
