using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialDefensiveScript : MonoBehaviour {
	
	public GameObject handFlipGesture;
	
	public GameObject handFistGesture;
	
	public Text displayMessage;

	public OffensiveAbilities offense;
	public DefensiveAbilities defense;
	
	public GameObject spawner;
	public GameObject goalPosition;

	public AudioClip ballistasAudio;
	public AudioClip oilSlicksAudio;
	public AudioClip tryOilSlicksAudio;
	public AudioClip explodeOilSlicksAudio;
	public AudioClip congratulationsAudio;

	public AudioSource audio;

	private bool ballistaHasKilled = false;
	private bool oilSlickHasSlowed = false;
	private bool oilSlickHasExploded = false;
	
	// Use this for initialization
	void Start () 
	{
		StartCoroutine (startTutorial ());
		StartCoroutine (activateOffensiveAbilities ());
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
	
	IEnumerator startTutorial()
	{
		displayMessage.text = "Now we will introduce the \n defensive phase. \n Use the hand flip attack \n to place a turret";
		audio.clip = ballistasAudio;
		audio.Play ();

		yield return new WaitForSeconds (6.0f);

		displayMessage.text = "Look at where you want to \n place your turret and \n perform the hand flip!";
		
		handFlipGesture.SetActive (true);

		yield return new WaitForSeconds (6.0f);

		displayMessage.text = "Look at where you want to \n place your turret and \n perform the hand flip!";

		offense.circularHandAttack = offense.handFlipAttack;
		offense.handFlipAttack = defense.placeTurretAttack;

		spawner.SetActive (true);
		TutorialSpawning spawn = (TutorialSpawning) spawner.GetComponent (typeof(TutorialSpawning));
		spawn.stopSpawning ();
		spawn.startSpawning ();
		goalPosition.SetActive (true);
		
	}
	
	IEnumerator activateOffensiveAbilities()
	{
		while(true)
		{
			yield return new WaitForSeconds(.1f);
			offense.controlCheck();
		}
	}

	IEnumerator beginOilSlickTutorial()
	{
		handFlipGesture.SetActive (false);
		offense.handFlipAttack = offense.emptyAttack;
		spawner.SetActive (false);
		goalPosition.SetActive (false);

		displayMessage.text = "You can also use the fist \n gesture to place oil slicks";
		audio.clip = oilSlicksAudio;
		audio.Play ();

		yield return new WaitForSeconds (audio.clip.length/2.0f);

		handFistGesture.SetActive (true);
		displayMessage.text = "The oil will slow down enemies";

		yield return new WaitForSeconds (audio.clip.length/2.0f);

		displayMessage.text = "Try slowing down the enemies";
		
		
		offense.fistAttack = defense.placeOilSlickAttack;

		spawner.SetActive (true);
		TutorialSpawning spawn = (TutorialSpawning) spawner.GetComponent (typeof(TutorialSpawning));
		spawn.stopSpawning ();
		spawn.startSpawning ();
		goalPosition.SetActive (true);

	}

	IEnumerator beginOilSlickExplosionTutorial()
	{
		spawner.SetActive (false);
		goalPosition.SetActive (false);

		GameObject[] spawnedZombies = GameObject.FindGameObjectsWithTag("Zombie");
		foreach (GameObject toDestroy in spawnedZombies) {
			GameObject.Destroy(toDestroy);
		}
		
		displayMessage.text = "Now, place another oil slick \n and use the fireball attack \n to set it on fire!";
		audio.clip = explodeOilSlicksAudio;
		audio.Play ();
		
		yield return new WaitForSeconds (audio.clip.length);
		
		offense.fistAttack = defense.placeOilSlickAttack;
		offense.handFlipAttack = offense.circularHandAttack;
		
	}

	IEnumerator endTutorial()
	{
		
		displayMessage.text = "Great Job!";
		audio.clip = congratulationsAudio;
		audio.Play();
		
		yield return new WaitForSeconds (audio.clip.length);
		
		gameObject.SetActive (false);
		
		Application.LoadLevel(Application.loadedLevel);
		
	}

	public void ballistaKilledZombie()
	{
		if (ballistaHasKilled == false) {
			ballistaHasKilled = true;
			GameObject[] placedBallistas = GameObject.FindGameObjectsWithTag("Ballista");
			foreach (GameObject toDestroy in placedBallistas) {
				GameObject.Destroy(toDestroy);
			}
			StartCoroutine (beginOilSlickTutorial ());
		}
	}

	public void oilSlickStoppedZombie()
	{
		if (oilSlickHasSlowed == false) {
			oilSlickHasSlowed = true;
			StartCoroutine (beginOilSlickExplosionTutorial ());
		}
	}

	public void oilSlickExploded()
	{
		print ("Oil Slick Exploded and call reached defensive tutorial script");
		if (oilSlickHasExploded == false) {
			oilSlickHasExploded = true;
			StartCoroutine (endTutorial());
		}

	}



}
