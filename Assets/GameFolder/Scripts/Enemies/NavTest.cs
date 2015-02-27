using UnityEngine;
using System.Collections;

public class NavTest : MonoBehaviour {

	public GameObject target;

	private NavMeshAgent agent;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent> ();;
		agent.SetDestination (target.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
