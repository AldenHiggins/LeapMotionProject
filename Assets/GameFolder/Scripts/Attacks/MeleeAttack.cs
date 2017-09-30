using UnityEngine;


public class MeleeAttack : AAttack
{
    private bool hasAttacked = false;

    public override void inactiveFunction() { }

    public override void releaseFunctionConcrete(Vector3 localPos, Vector3 worldPos, Quaternion localRot, Quaternion worldRot)
    {
        hasAttacked = false;
    }

    public override void holdFunctionConcrete(Vector3 localPos, Vector3 worldPos, Quaternion localRot, Quaternion worldRot)
    {
        if (!hasAttacked)
        {
            GetObjects.instance.getControllableUnit().doMeleeAttack();
            hasAttacked = true;
        }
    }
}


