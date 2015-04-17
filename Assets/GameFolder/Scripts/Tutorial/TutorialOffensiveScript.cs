using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialOffensiveScript : MonoBehaviour 
{	
	public GameObject offensiveButton;
	public GameObject defensiveButton;

	public GameObject handFlipGesture;
	public GameObject handFistGesture;
	
	public Text displayMessage;

	public GameObject flipEnemy;
	public GameObject fistEnemy;
	public GameObject zombie;
	
	public OffensiveAbilities offense;
	
	public AudioClip handFlipAudio;
	public AudioClip fireballAudio;
	public AudioClip fireballZombieAudio;
	public AudioClip fistAttackAudio;
	public AudioClip enemiesAudio;
	public AudioClip congratulationsAudio;
	public AudioSource audio;

	private bool activateOffense = false;

	public GameObject spawner;
	public GameObject goalPosition;

	private bool flipZombieKilled = false;
	private bool fistZombieKilled = false;
	private int zombiesKilled = 0;
	
	
	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void activateTutorial() 
	{
		handFlipGesture.SetActive (false);
		handFistGesture.SetActive (false);

		zombie.GetComponent<TutorialZombieKillCounter>().isOffense = true;
		zombiesKilled = 0;
		flipZombieKilled = false;
		fistZombieKilled = false;

		StartCoroutine (beginTutorial ());
		StartCoroutine (activateOffensiveAbilities ());
	
	}
	
	IEnumerator beginTutorial()
	{
		handFlipGesture.SetActive (true);
		displayMessage.text = "Here is an example of the \n most basic and important \n attack, the hand flip!";
		audio.clip = handFlipAudio;
		audio.Play();
		yield return new WaitForSeconds(audio.clip.length/2);

		offense.handFlipAttack = offense.alwaysFireballAttack;

		displayMessage.text = "Keep your hand in one place \n and rotate it \n just like the example.";
		
		yield return new WaitForSeconds(audio.clip.length/2);
		
		audio.clip = fireballAudio;
		audio.Play();
		displayMessage.text = "Now try and cast \n a fireball spell \n using the hand flip attack.";

		activateOffense = true;
		
		yield return new WaitForSeconds(audio.clip.length + 4);
		
		displayMessage.text = "Now look at the zombie and \n cast a fireball at him!";
		audio.clip = fireballZombieAudio;
		audio.Play();
		yield return new WaitForSeconds (audio.clip.length);
		
		flipEnemy.SetActive (true);
	}
	
	IEnumerator activateOffensiveAbilities()
	{
		while(true)
		{
			yield return new WaitForSeconds(.1f);
			if (activateOffense) 
			{
				offense.controlCheck();
			}
		}
	}

	IEnumerator beginFistTutorial()
	{
		offense.fistAttack = offense.clapAttack;
		offense.handFlipAttack = offense.emptyAttack;;
		
		handFlipGesture.SetActive (false);
		handFistGesture.SetActive (true);

		displayMessage.text = "Now we will introduce the \n second attack type, \n the fist attack!";
		audio.clip = fistAttackAudio;
		audio.Play();

		yield return new WaitForSeconds (4.0f);

		displayMessage.text = "Make a fist as shown \n have your hand face \n the camera and don't move it.";

		yield return new WaitForSeconds (4.0f);

		displayMessage.text = "Now try and kill the \n zombies with your fist!";

		yield return new WaitForSeconds (3.0f);

		fistEnemy.SetActive (true);

	}

	IEnumerator beginEnemiesTutorial()
	{
		offense.handFlipAttack = offense.alwaysFireballAttack;;
		
		handFlipGesture.SetActive (false);
		handFistGesture.SetActive (false);
		
		displayMessage.text = "During the game enemies \n will try and reach a goal.";
		audio.clip = enemiesAudio;
		audio.Play();
		
		yield return new WaitForSeconds (5.0f);
		
		displayMessage.text = "Use your attacks on them \n before they reach the \n other side to stop them";
		
		yield return new WaitForSeconds (4.0f);
		
		displayMessage.text = "Kill three zombies! \n Remember not to move \n your hand, only rotate or close it!";
		
		yield return new WaitForSeconds (5.0f);

		spawner.SetActive (true);
		TutorialSpawning spawn = (TutorialSpawning) spawner.GetComponent (typeof(TutorialSpawning));
		spawn.stopSpawning ();
		spawn.startSpawning ();
		goalPosition.SetActive (true);
		
	}

	IEnumerator endTutorial()
	{
		TutorialSpawning spawn = (TutorialSpawning) spawner.GetComponent (typeof(TutorialSpawning));
		spawn.stopSpawning ();
		spawner.SetActive (false);
		goalPosition.SetActive (false);

		displayMessage.text = "Great Job!";
		audio.clip = congratulationsAudio;
		audio.Play();
		
		yield return new WaitForSeconds (audio.clip.length);

		offense.handFlipAttack = offense.emptyAttack;
		offense.fistAttack = offense.emptyAttack;

		GameObject[] spawnedZombies = GameObject.FindGameObjectsWithTag("Zombie");
		foreach (GameObject toDestroy in spawnedZombies) {
			if (toDestroy.name != "Zombie(Clone)") continue;
			GameObject.Destroy(toDestroy);
		}

		offensiveButton.SetActive (true);
		defensiveButton.SetActive (true);

		gameObject.SetActive (false);
	}


	public void handFlipZombieKilled()
	{
		if (flipZombieKilled == false) {
			flipZombieKilled = true;
			StartCoroutine (beginFistTutorial ());
		}
	}

	public void handFistZombieKilled()
	{
		if (fistZombieKilled == false) {
			fistZombieKilled = true;
			StartCoroutine (beginEnemiesTutorial ());
		}
	}

	public void killedZombie() 
	{
		zombiesKilled++;

		if (zombiesKilled >= 3)
		{
			zombie.GetComponent<TutorialZombieKillCounter>().isOffense = false;
			StartCoroutine (endTutorial ());
		}

	}


}
