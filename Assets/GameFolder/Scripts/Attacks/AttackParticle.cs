using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackParticle : MonoBehaviour
{
    [SerializeField]
    private GameObject impactParticle;

    [SerializeField]
    private int attackDamage = 20;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

	void OnParticleCollision(GameObject other)
    {
		Rigidbody body = other.GetComponent<Rigidbody>();
        if (body == null)
        {
            return;
        }

        IUnit foundEnemy = body.gameObject.GetComponent<IUnit>();
        if (foundEnemy == null)
        {
            return;
        }

        // Get access to the collision data
        List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
        GetComponent<ParticleSystem>().GetCollisionEvents(other, collisionEvents);

        // If we have collided with an enemy deal the appropriate amount of damage
        foundEnemy.dealDamage(attackDamage, collisionEvents[0].velocity.normalized);

        // Spawn an impact particle if necessary
        if (impactParticle != null)
        {
            GameObject newImpactParticle = Instantiate(impactParticle, collisionEvents[0].intersection, Quaternion.identity);
            newImpactParticle.transform.parent = GetObjects.getAttackParticleContainer();
            newImpactParticle.SetActive(true);
        }

        // Finally destroy this particle
        Destroy(gameObject);
    }
}
