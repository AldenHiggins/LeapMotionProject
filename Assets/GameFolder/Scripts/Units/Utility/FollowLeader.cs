using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowLeader : MonoBehaviour
{
    private GameObject leader;

    private float followAngle;
    private float followDistance = 5.0f;

    void Start()
    {
        followAngle = Random.value * 180.0f;
        leader = GetObjects.getControllableUnit().gameObject;
    }

    public Vector3 getFollowPosition()
    {
        if (Vector3.Distance(transform.position, leader.transform.position) < followDistance + 1.0f)
        {
            return transform.position;
        }

        Vector3 followVector = (transform.position - leader.transform.position).normalized;
        followVector *= followDistance;
        followVector = Quaternion.Euler(0.0f, followAngle, 0.0f) * followVector;
        return leader.transform.position + followVector;
    }
}
