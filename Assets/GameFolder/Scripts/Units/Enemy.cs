using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Enemy : AUnit
{
    private GameObject goalPosition;

    override protected void initializeUnit()
    {
        goalPosition = GetObjects.getGoalPosition();
    }

    override protected void findTarget()
    {
        IUnit newTarget = findClosestUnit();

        if (newTarget != null)
        {
            target = newTarget;
        }
        else if (target == null)
        {
            target = goalPosition.GetComponent<IUnit>();
        }
    }
}
