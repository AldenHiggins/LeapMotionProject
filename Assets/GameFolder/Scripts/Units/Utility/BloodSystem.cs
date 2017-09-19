using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSystem : MonoBehaviour
{
    private IUnit parentUnit;

    private int currentParticle;

    // Use this for initialization
    void Start ()
    {
        parentUnit = transform.parent.GetComponent<IUnit>();
        if (parentUnit == null)
        {
            Debug.LogError("Health bar needs to be the child of an IUnit!");
            return;
        }

        parentUnit.installDamageListener(onDamageTaken);
    }

    void onDamageTaken(int damageTaken, Vector3 damageDirection)
    {
        transform.GetChild(currentParticle).gameObject.SetActive(false);
        // Rotate the blood system so that it emits outwards from the damage direction
        transform.GetChild(currentParticle).rotation = Quaternion.LookRotation(damageDirection);
        // Set the velocity of the blood accordingly in world space
        ParticleSystem.VelocityOverLifetimeModule particleVelocity = transform.GetChild(currentParticle).GetComponent<ParticleSystem>().velocityOverLifetime;
        Vector3 minVector = damageDirection * 0.1f;
        Vector3 maxVector = damageDirection * 1.5f;
        particleVelocity.x = new ParticleSystem.MinMaxCurve(minVector.x, maxVector.x);
        particleVelocity.y = new ParticleSystem.MinMaxCurve(minVector.y, maxVector.y);
        particleVelocity.z = new ParticleSystem.MinMaxCurve(minVector.z, maxVector.z);
        // Activate the blood particles
        transform.GetChild(currentParticle).gameObject.SetActive(true);

        // Switch to the next blood particle system
        currentParticle++;
        if (currentParticle >= transform.childCount)
        {
            currentParticle = 0;
        }
    }
}
