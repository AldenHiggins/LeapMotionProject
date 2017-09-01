using UnityEngine;

public class FireballAttack : AAttack
{
	public GameObject fireBall;

    public override void inactiveFunction() { }

    public override void releaseFunctionConcrete(Vector3 localPos, Vector3 worldPos, Quaternion localRot, Quaternion worldRot)
    {
        // Generate a fireball at the controller that fires out from the correct angle (so that the controller is a gun)
        GameObject newFireball = Instantiate(fireBall, worldPos, worldRot);
        newFireball.transform.parent = GetObjects.getAttackParticleContainer();
        newFireball.SetActive(true);
        newFireball.GetComponent<Renderer>().enabled = true;
    }

    public override void holdFunctionConcrete(Vector3 localPos, Vector3 worldPos, Quaternion localRot, Quaternion worldRot) { }
}


