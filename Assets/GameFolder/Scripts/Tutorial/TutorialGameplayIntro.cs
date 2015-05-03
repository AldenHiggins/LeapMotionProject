using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialGameplayIntro : MonoBehaviour 
{
	public bool activateOffense;
	public bool enableTurretPlacementSpots;
	public bool activateOilSlicks;

	public GameObject turretPlacementSpoots;

	public GameObject disableGesture;

	public GameObject newGesture;
	
	public Text displayMessage;
	public float firstMessageTime;
	
	public string secondMessage;
	public float secondMessageTime;
	
	public string thirdMessage;
	public float thirdMessageTime;
	
	public string fourthMessage;
	public float timeToKillEnemies;

	public GameObject nextGestureMessage;
	
	public bool switchToFist;

	public OffensiveAbilities offense;
	public DefensiveAbilities defense;

	public GameObject spawner;
	public GameObject goalPosition;

	public bool loadNextLevelAfter;


	
	// Use this for initialization
	void Start () 
	{
		StartCoroutine (messageFunction ());
		StartCoroutine (activateOffensiveAbilities ());
		disableGesture.SetActive (false);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
	
	IEnumerator messageFunction()
	{
		yield return new WaitForSeconds (firstMessageTime);
		if (newGesture != null)
			newGesture.SetActive (true);
		if (switchToFist)
		{
//			offense.fistAttack = offense.handFlipAttack;
//			offense.handFlipAttack = offense.circularHandAttack;
		}
			
		
		displayMessage.text = secondMessage.Replace("lineline","\n");
		yield return new WaitForSeconds (secondMessageTime);
		displayMessage.text = thirdMessage.Replace("lineline","\n");

		if (newGesture != null)
			newGesture.SetActive (false);

		spawner.SetActive (true);
		TutorialSpawning spawn = (TutorialSpawning) spawner.GetComponent (typeof(TutorialSpawning));
		spawn.stopSpawning ();
		spawn.startSpawning ();
		goalPosition.SetActive (true);

		if (enableTurretPlacementSpots)
		{
//			offense.handFlipAttack = defense.placeTurretAttack;
			//turretPlacementSpoots.SetActive(true);
		}

		if (activateOilSlicks)
		{
//			offense.fistAttack = defense.placeOilSlickAttack;
		}

		yield return new WaitForSeconds (thirdMessageTime);
		displayMessage.text = fourthMessage.Replace("lineline","\n");

		yield return new WaitForSeconds (timeToKillEnemies);

		if (loadNextLevelAfter)
			Application.LoadLevel(Application.loadedLevel);

		spawner.SetActive (false);
		goalPosition.SetActive (false);
		if (nextGestureMessage != null) 
			nextGestureMessage.SetActive (true);
		gameObject.SetActive (false);




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
