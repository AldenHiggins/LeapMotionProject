using UnityEngine;
using System.Collections;

public class PlaceBallista : AAttack
{
	public DefensiveAbilities defense;
	public GameObject ballista;
	public GameObject ballistaHead;
	public GameObject ballistaPending;
	private bool isInstantiated = false;
	private GameObject createdballista;
	
	
	public override void chargingFunction(HandModel[] hands){
		// Display prospective ballista spots
		// defense.showHideballistaPositions (true);
		//defense.highlightClosestballistaPlacementPosition();
		//print ("Place ballista is charging!");

		if (!isInstantiated) {
			createdballista = (GameObject)Instantiate (ballistaPending);
			isInstantiated = true;
		}
		createdballista.transform.position = defense.getRayHit().point;
	}
	
	public override void chargedFunction(HandModel[] hands){}
	
	public override void releaseFunction(HandModel[] hands){
			GameObject ballistaFinal = (GameObject)Instantiate (ballista);
			ballistaFinal.transform.position = defense.getRayHit().point ;
			Destroy (createdballista);
			((EmitterBehaviorBallista) ballistaHead.GetComponent(typeof(EmitterBehaviorBallista))).enabled = true;
			isInstantiated = false;
			//defense.ballistaUsed();
	}
	
	public override void holdGestureFunction(HandModel[] hands){}
	
	public override void inactiveFunction(HandModel[] hands){}
}

