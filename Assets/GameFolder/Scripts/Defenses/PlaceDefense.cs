using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlaceDefense : AAttack
{
	public DefensiveAbilities defense;
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

	void Start()
	{
		source = gameObject.GetComponent<AudioSource> ();
	}

    //public override void chargingFunction(HandModel[] hands)
    //{
    //    // Display prospective ballista spots
    //    // defense.showHideballistaPositions (true);
    //    //defense.highlightClosestballistaPlacementPosition();
    //    //print ("Place ballista is charging!");
    //    if (player.getCurrencyValue() >= defenseCost) 
    //    {
    //        if (!isInstantiated) 
    //        {
    //            createdDefensiveObject = (GameObject)Instantiate (defensiveObjectPending);
    //            createdDefensiveObject.SetActive(true);
    //            isInstantiated = true;
    //        }
    //        createdDefensiveObject.transform.position = defense.getRayHit ().point;
    //        Quaternion rotation= player.gameObject.transform.GetChild (1).GetChild (1).rotation;
    //        createdDefensiveObject.transform.rotation = Quaternion.Euler (0.0f, rotation.eulerAngles.y, 0.0f);
    //    }
    //}
	
    //public override void chargedFunction(HandModel[] hands){}
	
    //public override void releaseFunction(HandModel[] hands)
    //{
    //    if (player.getCurrencyValue() >= defenseCost) 
    //    {
    //        // Generate a popup to show how much gold was spent
    //        if (goldSpentUI != null)
    //        {
    //            GameObject thisDamage = (GameObject) Instantiate(goldSpentUI, defense.getRayHit().point, Quaternion.identity);
    //            thisDamage.SetActive(true);
				
    //            // Get the text field of the damage popup
    //            Text textFieldAmountOfDamage = thisDamage.transform.GetChild (1).GetChild(0).GetComponent<Text>();
    //            textFieldAmountOfDamage.text = "-" + defenseCost;
    //        }


    //        Quaternion rotation = player.gameObject.transform.GetChild (1).GetChild (1).rotation;
    //        GameObject ballistaFinal = (GameObject)Instantiate (defensiveObject, defense.getRayHit().point, Quaternion.Euler (0.0f, rotation.eulerAngles.y, 0.0f));
    //        ballistaFinal.SetActive (true);
    //        source.PlayOneShot(placeObjectSound);
    //        Destroy (createdDefensiveObject);
    //        isInstantiated = false;
    //        player.changeCurrency(-1 * defenseCost);
    //    }
    //}


    //public override void holdGestureFunction(HandModel[] hands){}
	
    //public override void inactiveFunction()
    //{
    //    if (isInstantiated)
    //    {
    //        Destroy(createdDefensiveObject);
    //        isInstantiated = false;
    //    }
    //}
}

