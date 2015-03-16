using UnityEngine;
using System.Collections;

public class PlaceOilSlick : AAttack
{
	public GameLogic game;
	public GameObject oilSlick;
	public GameObject oilSlickPending;
	private bool isInstantiated = false;
	private GameObject createdOilSlick;


	public override void chargingFunction(HandModel[] hands){

	}
	
	public override void chargedFunction(HandModel[] hands){}
	
	public override void releaseFunction(HandModel[] hands){
		RaycastHit hit = game.getRayHit();
		GameObject oilSlickFinal = (GameObject)Instantiate (oilSlick);
		oilSlickFinal.transform.position = hit.point;
		Destroy (createdOilSlick);
		OilSlick oilSlickScript = (OilSlick)oilSlickFinal.GetComponent (typeof(OilSlick));
		oilSlickScript.enabled = true;
		isInstantiated = false;
	}
	
	public override void holdGestureFunction(HandModel[] hands){
		if (!isInstantiated) {
			createdOilSlick = (GameObject)Instantiate (oilSlickPending);
			isInstantiated = true;
		}
		RaycastHit hit = game.getRayHit();
		createdOilSlick.transform.position = hit.point;
	}
	
	public override void inactiveFunction(HandModel[] hands){
	}
}
	