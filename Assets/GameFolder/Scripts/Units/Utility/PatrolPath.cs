using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPath : MonoBehaviour
{
    public GameObject patrolPointHolder;

    public bool backAndForthPath;

    private int currentPatrolPoint = -1;

    private int patrolDirection = 1;

    public int nextPatrolPoint()
    {
        // Switch directions if the path goes back and forth
        if (backAndForthPath)
        {
            if (patrolDirection == 1 && currentPatrolPoint == patrolPointHolder.transform.childCount - 1)
            {
                patrolDirection = -1;
            }
            else if (currentPatrolPoint == 0)
            {
                patrolDirection = 1;
            }
        }
        // Otherwise wrap the path back around to the start position
        else if (currentPatrolPoint == patrolPointHolder.transform.childCount - 1)
        {
            currentPatrolPoint = -1;
        }

        currentPatrolPoint = currentPatrolPoint + patrolDirection;

        return currentPatrolPoint;
    }
}
