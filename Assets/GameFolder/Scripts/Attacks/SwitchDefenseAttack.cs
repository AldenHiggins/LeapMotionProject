using UnityEngine;


public class SwitchDefenseAttack : AAttack
{
	private PlaceDefenseAttack defensiveAttack;

    public override void inactiveFunction(){}

    public override void releaseFunctionConcrete(Vector3 localPos, Vector3 worldPos, Quaternion localRot, Quaternion worldRot)
    {
        if (findDefensiveAttack())
        {
            defensiveAttack.switchDefense();
        }
    }

    public override void holdFunctionConcrete(Vector3 localPos, Vector3 worldPos, Quaternion localRot, Quaternion worldRot) { }

    private bool findDefensiveAttack()
    {
        if (defensiveAttack != null)
        {
            return true;
        }

        defensiveAttack = transform.parent.GetComponentInChildren<PlaceDefenseAttack>();
        return (defensiveAttack != null);
    }
}


