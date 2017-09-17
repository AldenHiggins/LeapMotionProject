using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAIController : MonoBehaviour
{
    private Behavior root;

	// Use this for initialization
	void Start ()
    {
        Leaf leaf1 = new Leaf(delegate ()
        {
            Debug.Log("Counting to 1");
            return BehaviorReturnCode.Success;
        });

        Leaf leaf2 = new Leaf(delegate ()
        {
            Debug.Log("Counting to 2");
            return BehaviorReturnCode.Success;
        });

        Leaf leaf3 = new Leaf(delegate ()
        {
            Debug.Log("Counting to 3");
            return BehaviorReturnCode.Success;
        });

        root = new Selector(leaf1, leaf2, leaf3);
	}
	
	// Update is called once per frame
	void Update ()
    {
        root.run();	
	}
}
