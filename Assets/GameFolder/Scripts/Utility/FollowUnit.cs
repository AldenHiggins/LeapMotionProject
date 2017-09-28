using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowUnit : MonoBehaviour
{
    private GameObject objectToMove;

    private Vector3 objectToMoveStartPosition;

    private Vector3 startingPosition;

	// Use this for initialization
	void Start ()
    {
        objectToMove = GetObjects.instance.getMovingObjectsContainer().gameObject;
        startingPosition = transform.position;
        objectToMoveStartPosition = objectToMove.transform.position;
    }
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 differenceVector = transform.position - startingPosition;
        objectToMove.transform.position = differenceVector + objectToMoveStartPosition;
	}
}
