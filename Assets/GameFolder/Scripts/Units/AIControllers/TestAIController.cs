using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestAIController : MonoBehaviour
{
    private Behavior root;
    private AUnit unit;

	// Use this for initialization
	void Start ()
    {
        // Get access to the unit we are controlling
        unit = GetComponent<AUnit>();

        Leaf leaf1 = new Leaf(delegate ()
        {
            // If the unit isn't moving on the nav mesh give it a destination
            if (unit.getIsPatrolling() == false)
            {
                Debug.Log("Finding new patrol position");

                Vector3 newTargetPosition = Vector3.zero;
                if (unit.RandomPoint(transform.position, unit.getPatrolSearchRange(), out newTargetPosition))
                {
                    // If we can't find a complete path to the target position return failure and try again
                    NavMeshPath path = new NavMeshPath();
                    if (!NavMesh.CalculatePath(transform.position, newTargetPosition, NavMesh.AllAreas, path))
                    {
                        return BehaviorReturnCode.Failure;
                    }
                    else if (path.status == NavMeshPathStatus.PathPartial)
                    {
                        return BehaviorReturnCode.Failure;
                    }

                    unit.getAnim().SetBool("Running", true);
                    unit.getAnim().SetBool("Attacking", false);

                    // Activate our nav mesh agent and start moving to our destination
                    if (!unit.getAgent().isActiveAndEnabled)
                    {
                        unit.getAgent().enabled = true;
                    }
                    unit.getAgent().isStopped = false;
                    unit.getAgent().SetDestination(newTargetPosition);

                    unit.setIsPatrolling(true);
                }
            }
            // If the unit is patrolling return success
            else
            {
                return BehaviorReturnCode.Success;
            }
            return BehaviorReturnCode.Success;
        });

        Leaf leaf2 = new Leaf(delegate ()
        {
            if (unit.isMovingNavMesh())
            {
                return BehaviorReturnCode.Running;
            }
            else
            {
                return BehaviorReturnCode.Success;
            }
        });

        Leaf leaf3 = new Leaf(delegate ()
        {
            unit.getAnim().SetBool("Running", false);
            unit.getAnim().SetBool("Attacking", false);
            unit.getAgent().isStopped = true;
            return BehaviorReturnCode.Success;
        });

        Leaf leaf4 = new Leaf(delegate ()
        {
            unit.setIsPatrolling(false);
            return BehaviorReturnCode.Success;
        });

        Timer stopTimer = new Timer(5.0f, leaf4);

        root = new Sequence(leaf1, leaf2, leaf3, stopTimer);
	}
	
	// Update is called once per frame
	void Update ()
    {
        root.run();	
	}
}
