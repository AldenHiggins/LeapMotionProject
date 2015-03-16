using UnityEngine;
using System.Collections;

public class PlaceOilSlick : AAttack
{
	public GameObject thisCamera;
	public GameObject oilSlick;
	public GameObject oilSlickPending;
	private bool isInstantiated = false;
	private GameObject createdOilSlick;


	public override void chargingFunction(HandModel[] hands){

	}
	
	public override void chargedFunction(HandModel[] hands){}
	
	public override void releaseFunction(HandModel[] hands){
		RaycastHit hit = getRayHit();
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
		RaycastHit hit = getRayHit();
		createdOilSlick.transform.position = hit.point;
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
	