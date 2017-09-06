using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private IUnit parentUnit;

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
        // Get a new scale for our green health bar based on the unit's current health %
        float newScale = (float)parentUnit.getCurrentHealth() / parentUnit.getMaxHealth();
        // Clamp it so we don't go into the negative scales
        newScale = Mathf.Max(0.0f, newScale);
        // Apply our scale to our green bar
        Vector3 oldScale = transform.GetChild(0).localScale;
        transform.GetChild(0).localScale = new Vector3(oldScale.x, newScale, oldScale.z);
    }
}
