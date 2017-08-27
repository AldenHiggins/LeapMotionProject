using UnityEngine;


public class SteamSwitchDefenses : SteamAttacks
{
	public PlaceDefenseSteam defensiveAttack;

    public override void inactiveFunction(){}

    public override void releaseFunction(OVRInput.Controller hand)
    {
        defensiveAttack.switchDefense();
    }

    public override void holdFunction(OVRInput.Controller hand){}
}


