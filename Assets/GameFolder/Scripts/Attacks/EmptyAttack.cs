using UnityEngine;

public class EmptyAttack : AAttack
{
    public override void inactiveFunction() { }

    public override void releaseFunctionConcrete(Vector3 localPos, Vector3 worldPos, Quaternion localRot, Quaternion worldRot) { }

    public override void holdFunctionConcrete(Vector3 localPos, Vector3 worldPos, Quaternion localRot, Quaternion worldRot) { }
}


