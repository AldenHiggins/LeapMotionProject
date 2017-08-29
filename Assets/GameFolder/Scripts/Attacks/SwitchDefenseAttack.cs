using UnityEngine;


public class SwitchDefenseAttack : AAttack
{
	public PlaceDefenseAttack defensiveAttack;

    public override void inactiveFunction(){}

    public override void releaseFunctionConcrete(Vector3 localPos, Vector3 worldPos, Quaternion localRot, Quaternion worldRot)
    {
        defensiveAttack.switchDefense();
    }

    public override void holdFunctionConcrete(Vector3 localPos, Vector3 worldPos, Quaternion localRot, Quaternion worldRot) { }
}


