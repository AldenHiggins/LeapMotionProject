using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialDefensiveScript : MonoBehaviour {

	public GameObject offensiveButton;
	public GameObject defensiveButton;

	public GameObject handFlipGesture;
	public GameObject handFistGesture;
	
	public Text displayMessage;

	public PlayerLogic player;
	//public OffensiveAbilities offense;
	//public DefensiveAbilities defense;

	public GameObject fireBall;
	
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

	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void activateTutorial() 
	{
		//player.changeCurrency (500);
		//ballistaHasKilled = false;
		//oilSlickHasSlowed = false;
		//oilSlickHasExploded = false;


		//handFlipGesture.SetActive (false);
		//handFistGesture.SetActive (false);

		//StartCoroutine (startTutorial ());
		////StartCoroutine (beginOilSlickTutorial ());
		//StartCoroutine (activateOffensiveAbilities ());

	}
	
	IEnumerator startTutorial()
	{
//		offense.handFlipAttack = offense.emptyAttack;
//		offense.fistAttack = offense.emptyAttack;

		displayMessage.text = "Now we will introduce the \n defensive phase. \n Use the hand flip attack \n to place a turret";
		audio.clip = ballistasAudio;
		audio.Play ();

		yield return new WaitForSeconds (6.0f);
		handFlipGesture.SetActive (true);
		yield return new WaitForSeconds (6.0f);

		displayMessage.text = "Look at where you want to \n place your turret and \n perform the hand flip!";
		
		handFlipGesture.SetActive (true);

		yield return new WaitForSeconds (6.0f);

		handFlipGesture.SetActive (false);

		//offense.circularHandAttack = offense.handFlipAttack;
//		offense.handFlipAttack = defense.placeTurretAttack;

		while (true) {
			GameObject[] placedBallistas = GameObject.FindGameObjectsWithTag("Ballista");
			if (placedBallistas.Length > 0) {
				print("Found a ballista in defensive ballista training");
				break;
			}
			yield return new WaitForSeconds(1.0f);
		}

		spawner.SetActive (true);
		goalPosition.SetActive (true);
		TutorialSpawning spawn = (TutorialSpawning) spawner.GetComponent (typeof(TutorialSpawning));
		spawn.stopSpawning ();
		spawn.startSpawning ();

		
	}
	
	IEnumerator activateOffensiveAbilities()
	{
		while(true)
		{
			yield return new WaitForSeconds(.1f);
			//offense.controlCheck();
		}
	}

	IEnumerator beginOilSlickTutorial()
	{
		yield return new WaitForSeconds(4.0f);
		
		GameObject[] spawnedZombies = GameObject.FindGameObjectsWithTag("Zombie");
		foreach (GameObject toDestroy in spawnedZombies) {
			if (toDestroy.name != "RootMotionZombie(Clone)") continue;
			GameObject.Destroy(toDestroy);
		}

		GameObject[] placedBallistas = GameObject.FindGameObjectsWithTag("Ballista");
		foreach (GameObject toDestroy in placedBallistas) {
			if (toDestroy.name != "BallistaComplete(Clone)") continue;
			GameObject.Destroy(toDestroy);
		}

		//player.changeCurrency (5);
		handFlipGesture.SetActive (false);
		spawner.SetActive (false);
		goalPosition.SetActive (false);

		displayMessage.text = "You can also use the fist \n gesture to place oil slicks";
		audio.clip = oilSlicksAudio;
		audio.Play ();

		yield return new WaitForSeconds (5.0f);

		handFistGesture.SetActive (true);
		displayMessage.text = "The oil will slow down enemies \n and also explode when hit \n with one of your fireballs.";

		yield return new WaitForSeconds (5.0f);

		displayMessage.text = "First, close your hand \n into a fist and look where \n you want to place the \n oil slick.";
		// TODO: 
//		offense.fistAttack = defense.placeOilSlickAttack;

		yield return new WaitForSeconds (5.0f);

		displayMessage.text = "Now open your fist \n to place it. Try placing some \n in the enemies' path.";

		while (true) {
			GameObject[] placedOilSlicks = GameObject.FindGameObjectsWithTag("OilSlick");
			if (placedOilSlicks.Length > 1) {
				print("Found an oil slick in defensive ballista training");
				break;
			}
			yield return new WaitForSeconds(1.0f);
		}

		spawner.SetActive (true);
		TutorialSpawning spawn = (TutorialSpawning) spawner.GetComponent (typeof(TutorialSpawning));
		spawn.stopSpawning ();
		spawn.startSpawning ();
		goalPosition.SetActive (true);

	}

	IEnumerator beginOilSlickExplosionTutorial()
	{
		handFistGesture.SetActive (false);

		spawner.SetActive (false);
		goalPosition.SetActive (false);
		
		displayMessage.text = "Now, watch what happens \n fire comes in contact \n with your oil slicks";

//		offense.fistAttack = defense.placeOilSlickAttack;
//		offense.handFlipAttack = offense.alwaysFireballAttack;


		//handFlipGesture.SetActive (true);

		GameObject[] placedSlicks = GameObject.FindGameObjectsWithTag("OilSlick");

		foreach (GameObject toDestroy in placedSlicks) {
			if (toDestroy.name != "TutorialOilSlick(Clone)") continue;
			else 
			{
				GameObject[] spawnedZombies = GameObject.FindGameObjectsWithTag("Zombie");
				foreach (GameObject blowupzombie in spawnedZombies) {
					if (blowupzombie.name != "RootMotionZombie(Clone)") continue;
					UnityEngine.AI.NavMeshAgent agent = blowupzombie.GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
					BasicEnemyController controller = blowupzombie.GetComponentInChildren<BasicEnemyController>();
					//controller.tutorialGoalTarget = toDestroy;
					agent.ResetPath();
					agent.SetDestination(toDestroy.transform.position);
					agent.acceleration = 8;
					agent.speed = 6;
					agent.Resume();
					print("Reset the target to oil slick");
				}

				audio.clip = explodeOilSlicksAudio;
				audio.Play ();
				
				yield return new WaitForSeconds (audio.clip.length);

				Vector3 fireballSpawnLocation = handFlipGesture.transform.position;
				Vector3 fireballVelocity = toDestroy.transform.position - handFlipGesture.transform.position;
				fireballVelocity *= 0.2f;
				fireballSpawnLocation += fireballVelocity.normalized * .8f;

				GameObject newFireball = 
					(GameObject) Instantiate(fireBall, fireballSpawnLocation, Quaternion.LookRotation(fireballVelocity.normalized));
				newFireball.SetActive(true); 
				//MoveFireball moveThis = (MoveFireball) newFireball.GetComponent(typeof(MoveFireball));
				//moveThis.setVelocity(fireballVelocity);
				//newFireball.GetComponent<Renderer>().enabled = true;
				//moveThis.setHash (0);

				break;
			}
		}



		yield return new WaitForSeconds (3.0f);

		handFlipGesture.SetActive (false);

//		offense.fistAttack.inactiveFunction ();
//		offense.fistAttack = offense.emptyAttack;
//		offense.handFlipAttack.inactiveFunction ();
//		offense.handFlipAttack = offense.emptyAttack;

	}

	IEnumerator endTutorial()
	{
		yield return new WaitForSeconds (3.0f);

		displayMessage.text = "Great Job!";
		audio.clip = congratulationsAudio;
		audio.Play();
		
		yield return new WaitForSeconds (audio.clip.length);

		GameObject[] placedSlicks = GameObject.FindGameObjectsWithTag("OilSlick");
		foreach (GameObject toDestroy in placedSlicks) {
			if (toDestroy.name != "TutorialOilSlick(Clone)") continue;
			GameObject.Destroy(toDestroy);
		}

		GameObject[] spawnedZombies = GameObject.FindGameObjectsWithTag("Zombie");
		foreach (GameObject toDestroy in spawnedZombies) {
			if (toDestroy.name != "RootMotionZombie(Clone)") continue;
			GameObject.Destroy(toDestroy);
		}

		offensiveButton.SetActive (true);
		defensiveButton.SetActive (true);

		gameObject.SetActive (false);

	}

	public void ballistaKilledZombie()
	{
		print ("ballistaKilledZombie() function called.");
		if (ballistaHasKilled == false) {
			ballistaHasKilled = true;
//			offense.handFlipAttack = offense.emptyAttack;

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
