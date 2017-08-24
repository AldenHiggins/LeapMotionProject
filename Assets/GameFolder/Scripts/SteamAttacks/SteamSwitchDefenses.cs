using UnityEngine;


public class SteamSwitchDefenses : SteamAttacks
{
	private int currentAttackIndex = 0;
	public GameObject attackChoices;
	public PlaceDefenseSteam defensiveAttack;

	//public override void inactiveFunction()
	//{

	//}
	
	//public override void releaseFunction(uint controllerIndex, SteamVR_TrackedObject trackedDevice)
	//{
	//	currentAttackIndex++;
	//	if (currentAttackIndex >= attackChoices.transform.childCount)
	//	{
	//		currentAttackIndex = 0;
	//	}

	//	defensiveAttack.switchDefense ();
	//}
	
	//public override void holdFunction(uint controllerIndex, SteamVR_TrackedObject trackedDevice)
	//{

	//}

}


