using UnityEngine;
using System.Collections;

public class NavTest : MonoBehaviour {

	public GameObject target;

	private UnityEngine.AI.NavMeshAgent agent;

	// Use this for initialization
	void Start () {
		agent = GetComponent<UnityEngine.AI.NavMeshAgent> ();;
		agent.SetDestination (target.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
