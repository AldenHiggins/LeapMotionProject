using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
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

        Leaf checkIsPatrolling = new Leaf(delegate ()
        {
            if (unit.patrolling == false)
            {
                return BehaviorReturnCode.Failure;
            }

            return BehaviorReturnCode.Success;
        });

        Leaf patrolPath = new Leaf(delegate ()
        {
            // Check if we have a patrol path to make use of
            PatrolPath path = unit.GetComponent<PatrolPath>();
            if (path != null)
            {
                unit.setDestination(path.patrolPointHolder.transform.GetChild(path.nextPatrolPoint()).position);
                unit.patrolling = true;
                return BehaviorReturnCode.Success;
            }
            return BehaviorReturnCode.Failure;
        });

        Leaf patrolArea = new Leaf(delegate ()
        {
            // If we have a patrol area use that to find our random position
            PatrolArea area = unit.GetComponent<PatrolArea>();
            if (area == null) return BehaviorReturnCode.Failure;

            // Generate our patrol position
            Vector3 newTargetPosition = Vector3.zero;
            Vector3 patrolPosition = area.patrolAreaObject.transform.position;
            float patrolRange = area.patrolAreaObject.transform.localScale.x;
            if (unit.findRandomPointOnNavMesh(patrolPosition, patrolRange, out newTargetPosition))
            {
                // Start moving towards our new random point
                unit.setDestination(newTargetPosition);
                unit.patrolling = true;
                return BehaviorReturnCode.Success;
            }
            return BehaviorReturnCode.Failure;
        });

        Leaf manualPatrol = new Leaf(delegate ()
        {
            // Check to see if the unit manually patrols
            ManualPatrol manualPatrolling = unit.GetComponent<ManualPatrol>();
            if (manualPatrolling == null) return BehaviorReturnCode.Failure;

            // If the unit manually patrols wait for a new patrol position
            if (manualPatrolling.getPatrolConsumed()) return BehaviorReturnCode.Running;

            // If we have a new patrol position consume it
            unit.setDestination(manualPatrolling.getPatrolPosition());
            unit.patrolling = true;
            return BehaviorReturnCode.Success;
        });

        Leaf patrolFollowLeader = new Leaf(delegate ()
        {
            // Check to see if the unit has a leader to follow
            FollowLeader leaderToFollow = unit.GetComponent<FollowLeader>();
            if (leaderToFollow == null) return BehaviorReturnCode.Failure;

            // If the unit has a leader follow it but don't start patrolling.  This allows
            // the unit to continuously follow the leader as it moves.
            unit.setDestination(leaderToFollow.getFollowPosition());
            return BehaviorReturnCode.Success;
        });

        Leaf patrolRandom = new Leaf(delegate ()
        {
            // If we don't have a defined path generate a random position
            Vector3 patrolCenterPosition = transform.position;
            float patrolRange = unit.patrolSearchRange;
            Vector3 newTargetPosition = Vector3.zero;

            // Generate our patrol position
            if (unit.findRandomPointOnNavMesh(patrolCenterPosition, patrolRange, out newTargetPosition))
            {
                // Start moving towards our new random point
                unit.setDestination(newTargetPosition);
                unit.patrolling = true;
                return BehaviorReturnCode.Success;
            }
            return BehaviorReturnCode.Failure;
        });

        Selector selectPatrolPath = new Selector(checkIsPatrolling, patrolPath, patrolArea, manualPatrol, patrolFollowLeader, patrolRandom);

        Leaf sendPatrolToSquad = new Leaf(delegate ()
        {
            SquadLeader squad = unit.GetComponent<SquadLeader>();
            if (squad == null) return BehaviorReturnCode.Success;

            // If the unit is part of a squad inform the subordinates of a new patrol position
            squad.issueNewTroopMovement();
            return BehaviorReturnCode.Success;
        });

        Leaf waitToReachDestination = new Leaf(delegate ()
        {
            if (unit.isMovingNavMesh() && unit.patrolling)
            {
                return BehaviorReturnCode.Running;
            }
            // If the unit is a follower then don't bother waiting to reach a destination
            else if (unit.GetComponent<FollowLeader>() != null)
            {
                return BehaviorReturnCode.Failure;
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

        Timer stopPatrollingTimer = new Timer(unit.patrolWaitTime, stopPatrolling);

        StateSequence patrolLogic = new StateSequence(selectPatrolPath, sendPatrolToSquad, waitToReachDestination, stopRunning, playIdleAnimTimer, stopPatrollingTimer);

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
                // If we find a new target stop patrolling
                unit.patrolling = false;
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
