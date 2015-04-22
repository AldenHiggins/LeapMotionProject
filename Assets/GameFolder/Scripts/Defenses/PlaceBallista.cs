using UnityEngine;
using System.Collections;

public class PlaceBallista : AAttack
{
	public DefensiveAbilities defense;
	public GameObject ballista;
	public GameObject ballistaHead;
	public GameObject ballistaPending;
	public AudioClip placeBallistaSound;
	private bool isInstantiated = false;
	private GameObject createdballista;
	private AudioSource source;

	void Start()
	{
		source = gameObject.GetComponent<AudioSource> ();
	}

	public override void chargingFunction(HandModel[] hands)
	{
		// Display prospective ballista spots
		// defense.showHideballistaPositions (true);
		//defense.highlightClosestballistaPlacementPosition();
		//print ("Place ballista is charging!");
		if (defense.getNumBallistasLeft () > 0) {
			if (!isInstantiated) {
				createdballista = (GameObject)Instantiate (ballistaPending);
				isInstantiated = true;
			}
			createdballista.transform.position = defense.getRayHit ().point;
		}
	}
	
	public override void chargedFunction(HandModel[] hands){}
	
	public override void releaseFunction(HandModel[] hands)
	{
		if (defense.getNumBallistasLeft () > 0) {
			GameObject ballistaFinal = (GameObject)Instantiate (ballista);
			ballistaFinal.SetActive (true);
			ballistaFinal.transform.position = defense.getRayHit ().point;
			source.PlayOneShot(placeBallistaSound);
			Destroy (createdballista);
			((EmitterBehaviorBallista)ballistaHead.GetComponent (typeof(EmitterBehaviorBallista))).enabled = true;
			isInstantiated = false;
			defense.ballistaUsed();
		}
		//defense.ballistaUsed();
	}
	
	public override void holdGestureFunction(HandModel[] hands){}
	
	public override void inactiveFunction()
	{
		if (isInstantiated) {
			Destroy(createdballista);
			isInstantiated = false;
		}
	}
}

