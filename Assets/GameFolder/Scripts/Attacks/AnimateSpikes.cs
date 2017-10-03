using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ProjectileDestroy))]
public class AnimateSpikes : MonoBehaviour
{
    public AnimationCurve curve;
    public float animationScalingFactor;
    private ProjectileDestroy projectileDestroy;
    private float currentTime;

	// Use this for initialization
	void Start ()
    {
        projectileDestroy = GetComponent<ProjectileDestroy>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Update the spikes based on the projectile's lifespan
        currentTime += Time.deltaTime / projectileDestroy.lifeSpan;
        currentTime = Mathf.Clamp(currentTime, 0.0f, 1.0f);

        // Get the new y position for all of the spikes
        float newPosition = curve.Evaluate(currentTime) * animationScalingFactor;

        // Apply the new position to the spikes
        for (int childIndex = 0; childIndex < transform.childCount; childIndex++)
        {
            Transform child = transform.GetChild(childIndex);
            if (child.childCount == 0) continue;
            child.GetChild(0).localPosition = new Vector3(0.0f, newPosition, 0.0f);
        }
	}
}
