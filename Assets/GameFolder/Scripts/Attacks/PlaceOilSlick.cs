using UnityEngine;
using System.Collections;

public class PlaceOilSlick : AAttack
{
	public DefensiveAbilities defense;
	private GameLogic game;
	private GameObject oilSlick;


	public override void chargingFunction(HandModel[] hands){
		oilSlick = (GameObject)Instantiate (defense.oilSlick);
	}
	
	public override void chargedFunction(HandModel[] hands){}
	
	public override void releaseFunction(HandModel[] hands){
	}
	
	public override void holdGestureFunction(HandModel[] hands){
		RaycastHit hit = game.getRayHit();
		oilSlick.transform.position = hit.point;
	}
	
	public override void inactiveFunction(HandModel[] hands){
		RaycastHit hit = game.getRayHit();
		oilSlick.transform.position = hit.point;
		OilSlick oilSlickScript = (OilSlick)oilSlick.GetComponent (typeof(OilSlick));
		oilSlickScript.enabled = true;
	}
}
	