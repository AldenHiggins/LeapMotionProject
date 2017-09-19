using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestAIController : MonoBehaviour
{
    private Behavior root;
    private Unit unit;

	// Use this for initialization
	void Start ()
    {
        // Get access to the unit we are controlling
        unit = GetComponent<Unit>();

        // Install an on death function to remove all logic from the unit
        unit.installDeathListener(delegate () { root = new Leaf(delegate () { return BehaviorReturnCode.Success; }); });

        ////////////////////////////////////////////////////////////////////
        /////////////////////   PATROLLING/IDLE   //////////////////////////
        ////////////////////////////////////////////////////////////////////
        Leaf getPatrolPath = new Leaf(delegate ()
        {
            // If the unit isn't moving on the nav mesh give it a destination
            if (unit.patrolling == false)
            {
                Vector3 newTargetPosition = Vector3.zero;
                if (unit.findRandomPointOnNavMesh(transform.position, unit.patrolSearchRange, out newTargetPosition))
                {
                    // Start moving towards our new random point
                    unit.setDestination(newTargetPosition);
                    unit.patrolling = true;
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
            unit.anim.SetBool("Running", false);
            unit.agent.isStopped = true;
            return BehaviorReturnCode.Success;
        });

        Leaf playIdleAnim = new Leaf(delegate ()
        {
            unit.anim.SetTrigger("PatrolIdleAnim1Trigger");
            return BehaviorReturnCode.Success;
        });

        RandomBehavior idleRand = new RandomBehavior(.1f, playIdleAnim);

        Timer playIdleAnimTimer = new Timer(1.0f, idleRand);

        Leaf stopPatrolling = new Leaf(delegate ()
        {
            unit.patrolling = false;
            return BehaviorReturnCode.Success;
        });

        Timer stopPatrollingTimer = new Timer(5.0f, stopPatrolling);

        StateSequence patrolLogic = new StateSequence(getPatrolPath, waitToReachDestination, stopRunning, playIdleAnimTimer, stopPatrollingTimer);

        ////////////////////////////////////////////////////////////////////
        /////////////////////   AGGRO/ATTACKING   //////////////////////////
        ////////////////////////////////////////////////////////////////////

        Leaf dropTargetLogic = new Leaf(delegate () 
        {
            // If we don't have a target continue
            if (unit.target == null) return BehaviorReturnCode.Success;

            // If our target is dying search for a new one
            if (unit.target.isUnitDying())
            {
                unit.target = null;
            }
            // If our target it outside of our aggro radius clear it
            if (unit.distanceToTarget() > unit.enemySearchRadius)
            {
                unit.target = null;
            }

            return BehaviorReturnCode.Success;
        });

        Leaf acquireTarget = new Leaf(delegate ()
        {
            // If we already have a living target just return success
            if (unit.target != null)
            {
                return BehaviorReturnCode.Success;
            }

            // Search for the closest enemy
            IUnit foundTarget = unit.findClosestTarget();

            // If we were able to find a target return success, otherwise we failed
            if (foundTarget != null)
            {
                unit.target = foundTarget;
                return BehaviorReturnCode.Success;
            }
            else
            {
                return BehaviorReturnCode.Failure;
            }
        });

        Leaf checkToAttack = new Leaf(delegate () 
        {
            // If we are already attacking continue
            if (unit.attacking) return BehaviorReturnCode.Success;

            // Check if our target is within the attack radius and isn't already dying
            if (unit.distanceToTarget() <= unit.attackRadius)
            {
                // Face the target
                Quaternion lookRot = Quaternion.LookRotation(unit.target.getGameObject().transform.position - unit.transform.position);
                unit.transform.rotation = Quaternion.Euler(0, lookRot.eulerAngles.y, 0);

                // Stop the navmesh agent
                unit.agent.isStopped = true;
                // Stop running
                unit.anim.SetBool("Running", false);
                // Play our attack animation
                unit.anim.SetTrigger("AttackTrigger");
                unit.attacking = true;
            }
            else
            {
                unit.attacking = false;
            }
            return BehaviorReturnCode.Success;
        });

        Leaf moveToTarget = new Leaf(delegate () 
        {
            // If we're still attacking wait to finish
            if (unit.attacking == true)
            {
                return BehaviorReturnCode.Success;
            }

            // Start moving towards our target
            unit.setDestination(unit.target.getGameObject().transform.position);
            return BehaviorReturnCode.Success;
        });

        Sequence attackSequence = new Sequence(dropTargetLogic, acquireTarget, checkToAttack, moveToTarget);

        ////////////////////////////////////////////////////////////////////
        //////////////////////////   ROOT   ////////////////////////////////
        ////////////////////////////////////////////////////////////////////

        root = new Selector(attackSequence, patrolLogic);
    }

    // Update is called once per frame
    void Update ()
    {
        root.run();	
	}
}
