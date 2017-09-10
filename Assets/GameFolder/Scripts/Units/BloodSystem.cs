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

    void onDamageTaken(int damageTaken)
    {
        transform.GetChild(currentParticle).gameObject.SetActive(false);
        transform.GetChild(currentParticle).gameObject.SetActive(true);

        currentParticle++;
        if (currentParticle >= transform.childCount)
        {
            currentParticle = 0;
        }
    }
}
