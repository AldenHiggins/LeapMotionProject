using UnityEngine;
using System.Collections;

public class TutorialWelcomeMessage : MonoBehaviour 
{

	public TutorialLogic tutorial;
	public GameObject nextMessage;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		HandModel[] hands = tutorial.getHands ();
		if (hands != null)
		{
			if (hands.Length > 0)
			{
				// Destroy this message and instantiate new one
				nextMessage.SetActive(true);
				Destroy (gameObject);
			}
		}
	}
}
