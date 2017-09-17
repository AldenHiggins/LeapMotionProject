using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAIController : MonoBehaviour
{
    private Behavior root;

	// Use this for initialization
	void Start ()
    {
        root = new Leaf(delegate () 
        {
            Debug.Log("Running our leaf");
            return BehaviorReturnCode.Success;
        });
	}
	
	// Update is called once per frame
	void Update ()
    {
        root.run();	
	}
}
