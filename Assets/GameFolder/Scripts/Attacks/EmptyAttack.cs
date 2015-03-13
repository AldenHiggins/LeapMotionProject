using UnityEngine;
using System.Collections;

public class EmptyAttack : AAttack 
{
	public override void chargingFunction(HandModel[] hands){}
	
	public override void chargedFunction(HandModel[] hands){}
	
	public override void releaseFunction(HandModel[] hands){}
	
	public override void holdGestureFunction(HandModel[] hands){}
	
	public override void inactiveFunction(HandModel[] hands){}
}
