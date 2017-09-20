using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPath : MonoBehaviour
{
    public GameObject patrolPointHolder;

    [HideInInspector]
    public int currentPatrolPoint = 0;

    public int nextPatrolPoint()
    {
        currentPatrolPoint++;

        if (currentPatrolPoint >= patrolPointHolder.transform.childCount)
        {
            currentPatrolPoint = 0;
        }

        return currentPatrolPoint;
    }
}
