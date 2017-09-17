using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections;

public class Enemy : AUnit
{
    //private GameObject goalPosition;

    //[SerializeField]
    //private float patrolSearchRange = 50.0f;

    override protected void initializeUnit()
    {
        //goalPosition = GetObjects.getGoalPosition();
    }

    //override protected void findTarget()
    //{
    //    IUnit newTarget = findClosestUnit();

    //    if (newTarget != null)
    //    {
    //        target = newTarget;
    //    }
    //    //else if (target == null)
    //    //{
    //    //    target = goalPosition.GetComponent<IUnit>();
    //    //}
    //}

    //bool RandomPoint(Vector3 center, float range, out Vector3 result)
    //{
    //    for (int i = 0; i < 30; i++)
    //    {
    //        Vector3 randomPoint = center + Random.insideUnitSphere * range;
    //        NavMeshHit hit;
    //        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
    //        {
    //            result = hit.position;
    //            return true;
    //        }
    //    }
    //    result = Vector3.zero;
    //    return false;
    //}

    //private void getNewPatrolPosition()
    //{
    //    Vector3 newPosition = new Vector3();

    //    if (RandomPoint(transform.position, patrolSearchRange, out newPosition))
    //    {

    //    }

    //}
}
