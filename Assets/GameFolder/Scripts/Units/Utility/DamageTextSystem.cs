using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextSystem : MonoBehaviour
{
    private IUnit parentUnit;
    private int currentTextIndex = 0;

    // Use this for initialization
    void Start()
    {
        parentUnit = transform.parent.GetComponent<IUnit>();
        if (parentUnit == null)
        {
            Debug.LogError("Damage text needs to be the child of an IUnit!");
            return;
        }

        parentUnit.installDamageListener(onDamageTaken);
    }

    void onDamageTaken(int damageTaken, Vector3 damageDirection)
    {
        // Activate the damage text
        transform.GetChild(currentTextIndex).GetComponent<DamageText>().activateText(damageTaken);

        // Iterate to the next text object
        currentTextIndex++;
        if (currentTextIndex >= transform.childCount)
        {
            currentTextIndex = 0;
        }
    }
}
