using UnityEngine;
using System.Collections;

public class PlaceTurret : AAttack
{
	public DefensiveAbilities defense;
	public GameObject turret;
	public GameObject turretPending;
	private bool isInstantiated = false;
	private GameObject createdturret;


	public override void chargingFunction(HandModel[] hands){
		// Display prospective turret spots
		// defense.showHideTurretPositions (true);
		//defense.highlightClosestTurretPlacementPosition();
		//print ("Place Turret is charging!");
		if (defense.getNumTurretsLeft () > 0) {
			if (!isInstantiated) {
				createdturret = (GameObject)Instantiate (turretPending);
				isInstantiated = true;
			}
			createdturret.transform.position = defense.getRayHit().point + new Vector3(0,1f,0);
		}
	}
	
	public override void chargedFunction(HandModel[] hands){}
	
	public override void releaseFunction(HandModel[] hands){
		if (defense.getNumSlicksLeft () > 0) {
			GameObject turretFinal = (GameObject)Instantiate (turret);
			turretFinal.transform.position = defense.getRayHit().point + new Vector3(0,1f,0);
			Destroy (createdturret);
			((EmitterBehavior) turretFinal.GetComponent(typeof(EmitterBehavior))).enabled = true;
			isInstantiated = false;
			defense.turretUsed();
		}
	}
	
	public override void holdGestureFunction(HandModel[] hands){}
	
	public override void inactiveFunction(HandModel[] hands){
		if (isInstantiated) {
			Destroy(createdturret);
			isInstantiated = false;
		}
	}
}
