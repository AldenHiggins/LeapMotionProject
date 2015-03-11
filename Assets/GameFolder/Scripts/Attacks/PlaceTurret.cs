using UnityEngine;
using System.Collections;

public class PlaceTurret : AAttack
{
	public DefensiveAbilities defense;

	public override void chargingFunction(HandModel[] hands){
		// Display prospective turret spots
		defense.showHideTurretPositions (true);
		defense.highlightClosestTurretPlacementPosition();
	}
	
	public override void chargedFunction(HandModel[] hands){}
	
	public override void releaseFunction(HandModel[] hands){
		defense.placeClosestTurret ();
	}
	
	public override void holdGestureFunction(HandModel[] hands){}
	
	public override void inactiveFunction(HandModel[] hands){}
}
