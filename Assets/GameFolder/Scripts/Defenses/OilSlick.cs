using UnityEngine;
using System.Collections;

public class OilSlick : MonoBehaviour {

	public GameObject boom;
	public bool isForTutorial;
	private AudioSource source;
	public AudioClip explosionSound;

	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource> ();
	}

	void OnTriggerEnter(Collider other) 
	{
		print(other.name + " collided with oil slick.");
		BasicEnemyController enemy = (BasicEnemyController) other.gameObject.GetComponentInChildren(typeof(BasicEnemyController));
		// If this isn't a root motion enemy (golem/cyclops)
		if (enemy == null)
		{
			enemy = (BasicEnemyController) other.gameObject.GetComponent(typeof(BasicEnemyController));
			if (enemy == null)
			{
				return;
			}
			NavMeshAgent agent = (NavMeshAgent) other.gameObject.GetComponent(typeof(NavMeshAgent));
			print("speed is " + agent.speed);
			agent.speed -= agent.speed *(0.5f);
		}
		// This is a root motion zombie
		else
		{
			Animator anim = other.gameObject.GetComponent<Animator>();
			anim.SetBool("Slowed", true);
		}


//		if (other.name == "CrazyFireball(Clone)") {
//			print("EXPLOSION!");
//			Vector3 boomPos = transform.position + new Vector3(0f,1f,0f);
//			Instantiate (boom, boomPos, Quaternion.identity);
//			Destroy(gameObject);
//		}

	}

	public void blowUp()
	{
		//print("EXPLOSION!");
		Vector3 boomPos = transform.position + new Vector3(0f,1f,0f);
		Instantiate (boom, boomPos, Quaternion.identity);
		source.PlayOneShot (explosionSound);
		if (isForTutorial) {
			gameObject.GetComponent<TutorialOilSlick>().hasBlownUp();
		}
		Destroy(gameObject);
	}
}
