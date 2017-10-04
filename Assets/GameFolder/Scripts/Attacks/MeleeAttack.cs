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

    private void Start()
    {
        meleeParticle = Instantiate(meleeParticle, meleeParticle.transform.position, meleeParticle.transform.rotation, GetObjects.instance.getControllableUnit().gameObject.transform);
        meleeParticle.SetActive(false);
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

        // Play the melee particle
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


