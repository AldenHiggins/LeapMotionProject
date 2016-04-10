using UnityEngine;


public class SteamSwitchDefenses : SteamAttacks
{
	private int currentAttackIndex = 0;
	public GameObject attackChoices;
	public PlaceDefenseSteam defensiveAttack;

	public override void inactiveFunction()
	{

	}
	
	public override void releaseFunction(uint controllerIndex, SteamVR_TrackedObject trackedDevice)
	{
		currentAttackIndex++;
		if (currentAttackIndex >= attackChoices.transform.childCount)
		{
			currentAttackIndex = 0;
		}
		SteamAttackContainer newAttackContainer = (SteamAttackContainer)attackChoices.transform.GetChild (currentAttackIndex).gameObject.GetComponent (typeof(SteamAttackContainer));

		//defensiveAttack.defensiveObject = newAttackContainer.defense;
		//defensiveAttack.defensiveObjectPending = newAttackContainer.defensePending;

		defensiveAttack.switchDefense ();

//		offense.rightHandFlipAttack = newAttackContainer.thisAttack;
//		offense.leftHandFlipAttack = newAttackContainer.thisAttack;
	}
	
	public override void holdFunction(uint controllerIndex, SteamVR_TrackedObject trackedDevice)
	{

	}

}


