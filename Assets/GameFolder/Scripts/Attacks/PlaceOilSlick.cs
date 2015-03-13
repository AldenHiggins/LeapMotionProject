using UnityEngine;
using System.Collections;

public class PlaceOilSlick : AAttack
{
	public GameLogic game;
	public GameObject oilSlick;
	private bool isInstantiated = false;


	public override void chargingFunction(HandModel[] hands){

	}
	
	public override void chargedFunction(HandModel[] hands){}
	
	public override void releaseFunction(HandModel[] hands){
		RaycastHit hit = game.getRayHit();
		oilSlick.transform.position = hit.point;
		OilSlick oilSlickScript = (OilSlick)oilSlick.GetComponent (typeof(OilSlick));
		oilSlickScript.enabled = true;
		isInstantiated = false;
	}
	
	public override void holdGestureFunction(HandModel[] hands){
		if (!isInstantiated) {
			oilSlick = (GameObject)Instantiate (oilSlick);
			isInstantiated = true;
		}
		RaycastHit hit = game.getRayHit();
		oilSlick.transform.position = hit.point;
	}
	
	public override void inactiveFunction(HandModel[] hands){
	}
}
	