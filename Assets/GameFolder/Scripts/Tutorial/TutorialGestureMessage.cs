using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialGestureMessage : MonoBehaviour 
{
	public GameObject gesture;

	public bool activateOffense;

	public Text displayMessage;
	public float firstMessageTime;

	public string secondMessage;
	public float secondMessageTime;

	public string thirdMessage;
	public float thirdMessageTime;

	public string fourthMessage;
	public GameObject enemyToSpawn;


	public OffensiveAbilities offense;

	public AudioClip handFlipAudio;
	public AudioClip fireballAudio;
	public AudioClip fireballZombieAudio;
	public AudioSource audio;


	// Use this for initialization
	void Start () 
	{
		if (gesture != null)
			gesture.SetActive (true);
		StartCoroutine (messageFunction ());
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	IEnumerator messageFunction()
	{
		audio.clip = handFlipAudio;
		audio.Play();
		yield return new WaitForSeconds(audio.clip.length/2);
		//yield return new WaitForSeconds (firstMessageTime);
		displayMessage.text = secondMessage.Replace("lineline","\n");

		yield return new WaitForSeconds(audio.clip.length/2);

		audio.clip = fireballAudio;
		audio.Play();
		displayMessage.text = thirdMessage.Replace("lineline","\n");

		if (activateOffense)
			StartCoroutine (activateOffensiveAbilities ());

		yield return new WaitForSeconds(audio.clip.length + 4);

		displayMessage.text = fourthMessage.Replace("lineline","\n");
		audio.clip = fireballZombieAudio;
		audio.Play();
		yield return new WaitForSeconds (audio.clip.length);

		enemyToSpawn.SetActive (true);
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
