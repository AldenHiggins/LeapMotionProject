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

        Leaf getPatrolPath = new Leaf(delegate ()
        {
            // If the unit isn't moving on the nav mesh give it a destination
            if (unit.getIsPatrolling() == false)
            {
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

        Leaf waitToReachDestination = new Leaf(delegate ()
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

        Leaf stopRunning = new Leaf(delegate ()
        {
            unit.getAnim().SetBool("Running", false);
            unit.getAgent().isStopped = true;
            return BehaviorReturnCode.Success;
        });

        Leaf playIdleAnim = new Leaf(delegate ()
        {
            unit.getAnim().SetTrigger("PatrolIdleAnim1Trigger");
            return BehaviorReturnCode.Success;
        });

        Timer playIdleAnimTimer = new Timer(1.0f, playIdleAnim);

        Leaf stopPatrolling = new Leaf(delegate ()
        {
            unit.setIsPatrolling(false);
            return BehaviorReturnCode.Success;
        });

        Timer stopPatrollingTimer = new Timer(5.0f, stopPatrolling);

        root = new StateSequence(getPatrolPath, waitToReachDestination, stopRunning, playIdleAnimTimer, stopPatrollingTimer);
	}
	
	// Update is called once per frame
	void Update ()
    {
        root.run();	
	}
}
