using UnityEngine;


public class ShadowBallAttack : AAttack
{
    [SerializeField]
    private GameObject shadowBall;

    [SerializeField]
    private Vector3 spawnPosition;

    private bool hasAttacked = false;

    public override void inactiveFunction(){}

    public override void releaseFunctionConcrete(Vector3 localPos, Vector3 worldPos, Quaternion localRot, Quaternion worldRot)
    {
        hasAttacked = false;
    }

    public override void holdFunctionConcrete(Vector3 localPos, Vector3 worldPos, Quaternion localRot, Quaternion worldRot)
    {
        if (!hasAttacked)
        {
            GetObjects.instance.getControllableUnit().castSpell(castShadowBall);
            hasAttacked = true;
        }
    }

    private void castShadowBall()
    {
        ControllableUnit unit = GetObjects.instance.getControllableUnit();
        Instantiate(shadowBall, unit.transform.TransformPoint(spawnPosition),
            unit.transform.rotation, GetObjects.instance.getAttackParticleContainer());
    }
}


