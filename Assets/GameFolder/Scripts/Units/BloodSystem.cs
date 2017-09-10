using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSystem : MonoBehaviour
{
    private IUnit parentUnit;

    [SerializeField]
    private GameObject bloodParticle;

    private ParticleSystem subParticle;

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

        subParticle = transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
    }

    void onDamageTaken(int damageTaken)
    {
        Debug.Log("Simulating blood!");
        subParticle.Stop();
        subParticle.Play();
    }
}
