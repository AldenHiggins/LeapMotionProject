using UnityEngine;

public class EmptySteamAttack : SteamAttacks
{
	public override void inactiveFunction(){}
	
	public override void releaseFunction(uint controllerIndex, GameObject trackedDevice){}
	
	public override void holdFunction(uint controllerIndex, GameObject trackedDevice){}
}


