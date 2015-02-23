using UnityEngine;
using System.Collections;

public class OilSlick : MonoBehaviour {

	public GameObject boom;

	// Use this for initialization
	void Start () {

	}

	void OnTriggerEnter(Collider other) {
		if (other.name == "NewFireball(Clone)") {
//			print(other.name + " collided with oil slick.");
			Vector3 boomPos = transform.position + new Vector3(0f,1f,0f);
			Instantiate (boom, boomPos, Quaternion.identity);
			Destroy(gameObject);
		}

	}
}
