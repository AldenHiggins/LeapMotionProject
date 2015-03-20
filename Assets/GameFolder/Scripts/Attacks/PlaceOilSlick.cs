using UnityEngine;
using System.Collections;

public class PlaceOilSlick : AAttack
{
	public GameObject thisCamera;
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
			oilSlickFinal.transform.position = getRayHit().point;
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
			createdOilSlick.transform.position = getRayHit().point;
		}
	}
	
	public override void inactiveFunction(HandModel[] hands){
	}

	private RaycastHit getRayHit()
	{
		int maskOne = 1 << 10;
		int maskTwo = 1 << 11;
		int mask = maskOne | maskTwo;
		Ray ray = new Ray (thisCamera.transform.position, thisCamera.transform.forward);
		RaycastHit hit;
		Physics.Raycast (ray, out hit, 100f, mask);
		return hit;
	}
}
	