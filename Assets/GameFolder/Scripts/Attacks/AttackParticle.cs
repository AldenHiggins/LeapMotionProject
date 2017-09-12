using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackParticle : MonoBehaviour
{
    [SerializeField]
    private GameObject impactParticle;
    [SerializeField]
    private GameObject muzzleParticle;

    [SerializeField]
    private int attackDamage = 20;

    [SerializeField]
    private float startingForce = 1000.0f;

	// Use this for initialization
	void Start ()
    {
        // Start out by adding force to the particle in its starting forward direction
        GetComponent<Rigidbody>().AddForce(transform.forward * startingForce);

        // Create a muzzle particle if applicable
        if (muzzleParticle)
        {
            muzzleParticle = Instantiate(muzzleParticle, transform.position, transform.rotation) as GameObject;
            muzzleParticle.transform.parent = GetObjects.getAttackParticleContainer();
            Destroy(muzzleParticle, 1.5f); // Lifetime of muzzle effect.
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnCollisionEnter(Collision hit)
    {
        GameObject other = hit.gameObject;

        // Deal damage if we hit an enemy
        IUnit foundEnemy = other.GetComponent<IUnit>();
        if (foundEnemy != null)
        {
            foundEnemy.dealDamage(attackDamage, hit.impulse.normalized);
        }

        // Spawn an impact particle if necessary
        if (impactParticle != null)
        {
            GameObject newImpactParticle = Instantiate(impactParticle, hit.contacts[0].point, Quaternion.identity);
            newImpactParticle.transform.parent = GetObjects.getAttackParticleContainer();
            newImpactParticle.SetActive(true);
        }

        // Finally destroy this particle
        Destroy(gameObject);
    }
}
