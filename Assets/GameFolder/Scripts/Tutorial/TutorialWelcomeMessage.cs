using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class TutorialWelcomeMessage : MonoBehaviour 
{

	public TutorialLogic tutorial;
	public AudioClip welcomeClip;
	public AudioSource audio;

	private GameObject nextMessage;
	private bool readyForHand = false;
	private bool isActive = false;

	public void setNextMessage(GameObject theMessage)
	{
		Debug.Log ("nextMessage set to " + theMessage.name);
		nextMessage = theMessage;
	}

	void Awake() {
		isActive = true;
	}

	// Use this for initialization
	void Start () {
		if (isActive) {
			StartCoroutine (tutorialStart ());
		}
	}

	IEnumerator tutorialStart() {
		audio.clip = welcomeClip;
		audio.Play();
		yield return new WaitForSeconds(audio.clip.length);
		readyForHand = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		HandModel[] hands = tutorial.getHands ();
		if ((hands != null) && readyForHand)
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
