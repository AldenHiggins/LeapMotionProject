using UnityEngine;
using System.Collections;

public class PlaceOilSlick : AAttack
{
	public GameObject oilSlick;
	public GameObject oilSlickPending;
	private bool isInstantiated = false;
	private GameObject createdOilSlick;
	public DefensiveAbilities defense;


	public override void chargingFunction(HandModel[] hands){

	}
	
	public override void chargedFunction(HandModel[] hands){}
	
	public override void releaseFunction(HandModel[] hands){
		if (defense.getNumSlicksLeft () > 0) {
			GameObject oilSlickFinal = (GameObject)Instantiate (oilSlick);
			oilSlickFinal.transform.position = defense.getRayHit().point;
			Destroy (createdOilSlick);
			OilSlick oilSlickScript = (OilSlick)oilSlickFinal.GetComponent (typeof(OilSlick));
			oilSlickScript.enabled = true;
			isInstantiated = false;
			defense.slickUsed();
		}
	}

	public override void holdGestureFunction(HandModel[] hands){
		if (defense.getNumSlicksLeft () > 0) {
			if (!isInstantiated) {
				createdOilSlick = (GameObject)Instantiate (oilSlickPending);
				isInstantiated = true;
			}
			createdOilSlick.transform.position = defense.getRayHit().point;
		}
	}
	
	public override void inactiveFunction(HandModel[] hands){
		if (isInstantiated){
			isInstantiated = false;
			Destroy (createdOilSlick);
		}
	}
}
	