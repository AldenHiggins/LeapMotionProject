using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class TutorialWelcomeMessage : MonoBehaviour 
{

	public Text textBox;
	public TutorialLogic tutorial;
	public AudioClip welcomeClip;
	public AudioSource audio;

	public GameObject offensiveButton;
	public GameObject defensiveButton;

	private bool readyForHand = false;
	private bool isActive = false;
	
	void Awake() 
	{
		isActive = true;
	}

	// Use this for initialization
	void Start () 
	{
		if (isActive) {
			StartCoroutine (tutorialStart ());
		}
	}

	IEnumerator tutorialStart() 
	{
		audio.clip = welcomeClip;
		audio.Play();
		yield return new WaitForSeconds(audio.clip.length);
		readyForHand = true;
	}

	IEnumerator presentChoices() 
	{
		textBox.text = "Now reach out and \n touch your selection to start\n the appropriate tutorial.";

		yield return new WaitForSeconds(3);

		offensiveButton.SetActive(true);
		defensiveButton.SetActive(true);
		
		Destroy (gameObject);
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
				StartCoroutine(presentChoices());
			}
		}
	}
}
