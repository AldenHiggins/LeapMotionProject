using UnityEngine;


public class SteamSwitchDefenses : SteamAttacks
{
	private int currentAttackIndex = 0;
	public GameObject attackChoices;
	public PlaceDefenseSteam defensiveAttack;

    public override void inactiveFunction()
    {

    }

    public override void releaseFunction(OVRInput.Controller hand)
    {
        currentAttackIndex++;
        if (currentAttackIndex >= attackChoices.transform.childCount)
        {
            currentAttackIndex = 0;
        }

        defensiveAttack.switchDefense();
    }

    public override void holdFunction(OVRInput.Controller hand)
    {

    }

}


