using UnityEngine;
using System.Collections;

public class CrazyFireball : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnParticleCollision(GameObject other) {
		Rigidbody body = other.GetComponent<Rigidbody>();
		if (body) {
            BasicEnemyController foundEnemy = body.gameObject.GetComponent<BasicEnemyController>();
            
            if (foundEnemy != null)
            {
                foundEnemy.dealDamage(20);    
            }
		}
	}
}
