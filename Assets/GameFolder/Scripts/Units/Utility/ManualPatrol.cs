using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualPatrol : MonoBehaviour
{
    private bool patrolConsumed;

    private Vector3 nextPatrolPosition;

	// Use this for initialization
	void Start ()
    {
        patrolConsumed = true;
	}

    public Vector3 getPatrolPosition()
    {
        patrolConsumed = true;
        return nextPatrolPosition;
    }

    public void setPatrolPosition(Vector3 newPosition)
    {
        patrolConsumed = false;
        nextPatrolPosition = newPosition;
    }

    public bool getPatrolConsumed()
    {
        return patrolConsumed;
    }
}
