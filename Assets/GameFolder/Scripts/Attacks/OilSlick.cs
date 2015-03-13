using UnityEngine;
using System.Collections;

public class OilSlick : MonoBehaviour {

	public GameObject boom;

	// Use this for initialization
	void Start () {
	}

	void OnTriggerEnter(Collider other) {
		BasicEnemyController enemy = (BasicEnemyController) other.gameObject.GetComponent(typeof(BasicEnemyController));
		print(other.name + " collided with oil slick.");
		if (other.name == "CrazyFireball(Clone)") {
			print("EXPLOSION!");
			Vector3 boomPos = transform.position + new Vector3(0f,1f,0f);
			Instantiate (boom, boomPos, Quaternion.identity);
			Destroy(gameObject);
		}
		if ( enemy!= null ){
			NavMeshAgent agent = (NavMeshAgent) other.gameObject.GetComponent(typeof(NavMeshAgent));
			print("speed is " + agent.speed);
			agent.speed -= agent.speed *(0.5f);
		}

	}
}
