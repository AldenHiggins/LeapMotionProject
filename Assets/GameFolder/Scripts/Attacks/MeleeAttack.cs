using UnityEngine;


public class MeleeAttack : AAttack
{
    [SerializeField]
    private AudioClip meleeNoise;
    [SerializeField]
    private GameObject meleeParticle;
    [SerializeField]
    private int meleeDamage;

    private bool hasAttacked = false;
    private int meleeAttackNumber = 0;
    private bool particleInitialized = false;

    private void createMeleeParticle()
    {
        ControllableUnit unit = GetObjects.instance.getControllableUnit();
        if (unit == null) return;
        meleeParticle = Instantiate(meleeParticle, meleeParticle.transform.position, meleeParticle.transform.rotation, unit.gameObject.transform);
        meleeParticle.SetActive(false);
        particleInitialized = true;
    }

    public override void inactiveFunction() { }

    public override void releaseFunctionConcrete(Vector3 localPos, Vector3 worldPos, Quaternion localRot, Quaternion worldRot)
    {
        hasAttacked = false;
    }

    public override void holdFunctionConcrete(Vector3 localPos, Vector3 worldPos, Quaternion localRot, Quaternion worldRot)
    {
        if (!hasAttacked)
        {
            GetObjects.instance.getControllableUnit().doMeleeAttack(meleeAttackNumber, meleeAnimationEvent);
            hasAttacked = true;
        }
    }

    private void meleeAnimationEvent()
    {
        // Access the unit
        ControllableUnit unit = GetObjects.instance.getControllableUnit();
        // Deal melee damage
        unit.doMeleeDamage(1.0f, meleeDamage);

        // Play the melee particle, initialize it if needed
        if (!particleInitialized)
        {
            createMeleeParticle();
        }

        meleeParticle.SetActive(false);
        meleeParticle.SetActive(true);

        // Play the melee sound if we have one
        if (meleeNoise)
        {
            unit.getSource().PlayOneShot(meleeNoise);
        }

        // Increment the attack number so we keep playing different animations
        meleeAttackNumber++;
        if (meleeAttackNumber > 2)
        {
            meleeAttackNumber = 0;
        }
    }
}


