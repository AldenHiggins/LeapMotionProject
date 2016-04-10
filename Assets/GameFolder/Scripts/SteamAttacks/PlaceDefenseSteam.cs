using UnityEngine;
using UnityEngine.UI;

public class PlaceDefenseSteam : SteamAttacks
{
	public PlayerLogic player;
	public int defenseCost;
	public GameObject defensiveObject;
	public GameObject defensiveObjectPending;
	public AudioClip placeObjectSound;
	private bool isInstantiated = false;
	private GameObject createdDefensiveObject;
	private AudioSource source;
	public bool rotateWhilePlacing;
	public GameObject goldSpentUI;

//	public RayCastTest rayCast;
	
	void Start()
	{
		source = gameObject.GetComponent<AudioSource> ();
	}

	public override void inactiveFunction()
	{
		if (isInstantiated)
		{
			Destroy(createdDefensiveObject);
			isInstantiated = false;
		}
	}
	
	public override void releaseFunction(uint controllerIndex, SteamVR_TrackedObject trackedDevice)
	{
		Debug.Log ("Casting release function");
		if (player.getCurrencyValue() >= defenseCost) 
		{
			// Generate a popup to show how much gold was spent
			if (goldSpentUI != null)
			{
//				GameObject thisDamage = (GameObject) Instantiate(goldSpentUI, rayCast.defensiveRayHit(controllerIndex).point, Quaternion.identity);
//				thisDamage.SetActive(true);
//				
//				// Get the text field of the damage popup
//				Text textFieldAmountOfDamage = thisDamage.transform.GetChild (1).GetChild(0).GetComponent<Text>();
//				textFieldAmountOfDamage.text = "-" + defenseCost;
			}
			
			
//			Quaternion rotation = trackedDevice.transform.rotation;
//			GameObject ballistaFinal = (GameObject)Instantiate (defensiveObject, rayCast.defensiveRayHit(controllerIndex).point, Quaternion.Euler (0.0f, rotation.eulerAngles.y, 0.0f));
//			ballistaFinal.SetActive (true);
//			source.PlayOneShot(placeObjectSound);
//			Destroy (createdDefensiveObject);
//			isInstantiated = false;
//			player.changeCurrency(-1 * defenseCost);
		}
	}
	
	public override void holdFunction(uint controllerIndex, SteamVR_TrackedObject trackedDevice)
	{
		// Display prospective ballista spots
		// defense.showHideballistaPositions (true);
		//defense.highlightClosestballistaPlacementPosition();
		//print ("Place ballista is charging!");
		if (player.getCurrencyValue() >= defenseCost) 
		{
//			if (!isInstantiated) 
//			{
//				createdDefensiveObject = (GameObject)Instantiate (defensiveObjectPending);
//				createdDefensiveObject.SetActive(true);
//				isInstantiated = true;
//			}
//			createdDefensiveObject.transform.position = rayCast.defensiveRayHit(controllerIndex).point;
//			Quaternion rotation= trackedDevice.transform.rotation;
//			createdDefensiveObject.transform.rotation = Quaternion.Euler (0.0f, rotation.eulerAngles.y, 0.0f);
		}
	}

	public void switchDefense()
	{
		if (isInstantiated)
		{
			isInstantiated = false;
			Destroy (createdDefensiveObject);
		}
	}


}


