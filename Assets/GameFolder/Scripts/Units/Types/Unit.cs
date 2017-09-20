using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour, IUnit
{
    [Header("Unit Attributes")]
    public int startingHealth = 100;
    public float attackRadius = 2.0f;
    public int attackDamage = 1;
    public float bodyDespawnTime = 10.0f;
    public float headshotHeight = 1.5f;
    public float enemySearchRadius = 10.0f;

    [Header("Patrolling")]
    public float patrolSearchRange = 50.0f;
    public float patrolDestinationReachedRange = 1.0f;
    public float patrolWaitTime = 5.0f;

    [Header("Team")]
    public bool isAlly = false;

    [Header("Audio")]
    public AudioClip woundSound;
    public AudioClip killSound;

    // UNIT STATE
    [HideInInspector]
    public int health;
    [HideInInspector]
    public bool attacking = false;
    [HideInInspector]
    public IUnit target;
    [HideInInspector]
    public Vector3 targetPosition;
    [HideInInspector]
    public bool patrolling;
    [HideInInspector]
    public bool isDying = false;

    // UNIT INFO
    [HideInInspector]
    public int enemySearchLayer = (1 << 16) | (1 << 11);
    private int allyLayers = (1 << 16) | (1 << 11);
    private int enemyLayers = (1 << 8);

    // COMPONENTS
    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public UnityEngine.AI.NavMeshAgent agent;
    [HideInInspector]
    public AudioSource source;
    [HideInInspector]
    public Collider unitCollider;

    // DELEGATES
    private Action onDeath;
    private Action<int, Vector3> onDamageTaken;

    /////////////////////////////////////////////////////////
    //////////////////  INITIALIZATION  /////////////////////
    /////////////////////////////////////////////////////////

    void Start()
    {
        // Register the unit's components
        anim = transform.GetChild(0).gameObject.GetComponent<Animator>();
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }
        agent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        source = GetComponent<AudioSource>();
        unitCollider = GetComponent<Collider>();

        // Initialize things
        health = startingHealth;
        agent.updateRotation = true;
        if (isAlly)
        {
            enemySearchLayer = enemyLayers;
        }
        else
        {
            enemySearchLayer = allyLayers;
        }
    }

    /////////////////////////////////////////////////////////
    ///////////////  ANIMATION CALLBACKS  ///////////////////
    /////////////////////////////////////////////////////////

    public void attackAnimDone()
    {
        attacking = false;
    }

    public void damageTarget()
    {
        // Check if we have a target
        if (target == null) return;

        // Check to see if our target has died
        if (target.isUnitDying()) return;

        // If we are still in range deal damage to the target
        Vector3 distance = target.getGameObject().transform.position - transform.position;
        distance.y = 0.0f;
        if (distance.magnitude <= attackRadius)
        {
            target.dealDamage(attackDamage, distance.normalized);
        }
    }

    /////////////////////////////////////////////////////////
    /////////////////  HELPER FUNCTIONS  ////////////////////
    /////////////////////////////////////////////////////////

    public float distanceToTarget()
    {
        if (target == null) return 0.0f;

        return Vector3.Distance(target.getGameObject().transform.position, transform.position);
    }

    public IUnit findClosestTarget()
    {
        IUnit foundTarget = null;
        // Sphere cast around the unit for a target
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, enemySearchRadius, enemySearchLayer);
        float minimumDistance = float.MaxValue;
        // Find the closest enemy
        for (int hitIndex = 0; hitIndex < hitColliders.Length; hitIndex++)
        {
            IUnit enemyUnit = (IUnit)hitColliders[hitIndex].gameObject.GetComponent(typeof(IUnit));
            if (enemyUnit != null && !enemyUnit.isUnitDying())
            {
                float enemyDistance = Vector3.Distance(enemyUnit.getGameObject().transform.position, gameObject.transform.position);
                if (enemyDistance < minimumDistance)
                {
                    foundTarget = enemyUnit;
                    minimumDistance = enemyDistance;
                }
            }
        }
        return foundTarget;
    }

    public void setDestination(Vector3 newDestination)
    {
        // If our new destination is close enough to where we currently are don't do anything
        if (Vector3.Distance(transform.position, newDestination) <= 1.0f)
        {
            // Stop the navmesh agent
            agent.isStopped = true;
            // Stop running
            anim.SetBool("Running", false);
            return;
        }

        anim.SetBool("Running", true);

        // Set our target position
        targetPosition = newDestination;

        // Activate our nav mesh agent and start moving to our destination
        if (!agent.isActiveAndEnabled)
        {
            agent.enabled = true;
        }
        agent.isStopped = false;
        agent.SetDestination(newDestination);
    }

    public bool findRandomPointOnNavMesh(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + UnityEngine.Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                // Check if we can find a path to the target position before returning it
                NavMeshPath path = new NavMeshPath();
                if (!NavMesh.CalculatePath(transform.position, hit.position, NavMesh.AllAreas, path))
                {
                    continue;
                }
                else if (path.status == NavMeshPathStatus.PathPartial)
                {
                    continue;
                }
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    // Determine if the unit is moving on the nav mesh and has a destination
    public bool isMovingNavMesh()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    return false;
                }
            }
        }
        return true;
    }

    /////////////////////////////////////////////////////////
    //////////////////  STATUS EFFECTS  /////////////////////
    /////////////////////////////////////////////////////////

    public void slowDown()
    {
        agent.speed = 1;
    }

    /////////////////////////////////////////////////////////
    /////////////////    DAMAGE/DEATH    ////////////////////
    /////////////////////////////////////////////////////////

    // Have the unit take damage
    public void dealDamage(int damage, Vector3 damageVector)
    {
        health -= damage;

        if (onDamageTaken != null)
        {
            onDamageTaken(damage, damageVector);
        }

        if (health > 0)
        {
            // Getting attacked interrupts attacking
            attacking = false;
            // Play the wound animation/sound
            anim.SetTrigger("WoundTrigger");
            source.PlayOneShot(woundSound);
        }
        else
        {
            source.PlayOneShot(killSound);
            kill();
        }
    }

    void kill()
    {
        isDying = true;
        if (agent.enabled)
        {
            agent.enabled = false;
        }
        // Call our on death callbacks if necessary
        if (onDeath != null)
        {
            onDeath();
        }
        unitCollider.enabled = false;
        // Clear other triggers so the unit doesn't do anything other than play the death animation
        anim.ResetTrigger("WoundTrigger");
        anim.ResetTrigger("AttackTrigger");
        // Trigger the death animation
        anim.SetTrigger("DeathTrigger");
        StartCoroutine(despawnBody());
    }

    IEnumerator despawnBody()
    {
        yield return new WaitForSeconds(bodyDespawnTime);
        Destroy(gameObject);
    }

    /////////////////////////////////////////////////////////
    /////////////////  GETTERS/SETTERS  /////////////////////
    /////////////////////////////////////////////////////////

    public GameObject getGameObject()
    {
        return gameObject;
    }

    public bool isUnitAlly()
    {
        return isAlly;
    }

    public int getMaxHealth()
    {
        return startingHealth;
    }

    public int getCurrentHealth()
    {
        return health;
    }

    public bool isUnitDying()
    {
        return isDying;
    }

    public void installDeathListener(Action onDeathCallback)
    {
        onDeath += onDeathCallback;
    }

    public void installDamageListener(Action<int, Vector3> onDamageCallback)
    {
        onDamageTaken += onDamageCallback;
    }
}
