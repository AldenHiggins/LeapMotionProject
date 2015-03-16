using UnityEngine;
using System.Collections;

public class TutorialPlaceTurret : AAttack
{
	public DefensiveAbilities defense;
	
	public override void chargingFunction(HandModel[] hands){
		// Display prospective turret spots
		// defense.showHideTurretPositions (true);
		defense.highlightClosestTurretPlacementPosition();
		//print ("Place Turret is charging!");
	}
	
	public override void chargedFunction(HandModel[] hands){}
	
	public override void releaseFunction(HandModel[] hands){
		defense.tutorialPlaceClosestTurret ();
		//print ("Turret placed!");
	}
	
	public override void holdGestureFunction(HandModel[] hands){}
	
	public override void inactiveFunction(HandModel[] hands){}
}
