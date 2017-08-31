using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AUnit : MonoBehaviour, IUnit
{
    // UNIT CHARACTERISTICS
    [SerializeField]
    protected int startingHealth = 100;
    [SerializeField]
    protected float attackRadius = 2.0f;
    [SerializeField]
    protected int attackDamage = 1;
    [SerializeField]
    protected float bodyDespawnTime = 10.0f;
    // ALLY/ENEMY
    protected bool isAlly = false;
    // HEADSHOTS
    [SerializeField]
    protected float headshotHeight = 1.5f;
    // ANIMATOR
    protected Animator anim;
    protected int health;
    protected bool attacking = false;
    // MOVEMENT
    protected UnityEngine.AI.NavMeshAgent agent;
    protected GameObject target;
    protected Vector3 velocity;
    // ENEMY SEARCH
    protected int enemySearchLayer = (1 << 16) | (1 << 11);
    protected float enemySearchRadius = 3.0f;
    // AUDIO
    protected AudioSource source;
    [SerializeField]
    protected AudioClip woundSound;
    [SerializeField]
    protected AudioClip killSound;
    // RAGDOLL/DEATH
    protected bool isDying = false;

    // Use this for initialization
    void Start()
    {
        // Register the unit's components
        anim = transform.GetChild(0).gameObject.GetComponent<Animator>();
        agent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        source = GetComponent<AudioSource>();

        health = startingHealth;
        agent.updateRotation = true;

        initializeUnit();
    }

    abstract protected void initializeUnit();

    // Update is called once per frame
    void Update()
    {
        // If the unit is dying don't do anything
        if (health <= 0)
        {
            return;
        }

        // Search for a new target
        findTarget();

        // If the unit has no target idle
        if (target == null)
        {
            // Stop running
            anim.SetBool("Running", false);
            agent.isStopped = true;
            return;
        }

        // Determine if the unit should start attacking its target
        Vector3 targetVector = target.transform.position - transform.position;
        targetVector.y = 0.0f;
        float distanceToTarget = targetVector.magnitude;

        // If the enemy is outside attack range keep coming forward
        if (velocity.magnitude > attackRadius)
        {
            moveToTarget();
        }
        // If the target is in range attack it
        else
        {
            attackTarget();
        }
    }

    protected virtual void moveToTarget()
    {
        // If we're still attacking wait to finish
        if (attacking == true)
        {
            return;
        }

        // Animate the unit to start running
        anim.SetBool("Running", true);

        // Activate our nav mesh agent and start moving to our destination
        if (!agent.isActiveAndEnabled)
        {
            agent.enabled = true;
        }
        agent.isStopped = false;
        agent.SetDestination(target.transform.position);
    }

    protected virtual void attackTarget()
    {
        // Stop running
        anim.SetBool("Running", false);
        agent.isStopped = true;

        // Face the target
        transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);

        // If the unit isn't attacking yet start the attack function
        if (attacking == false)
        {
            attacking = true;
            StartCoroutine(attack());
        }
    }

    protected virtual void findTarget()
    {
        GameObject newTarget = findClosestUnit();

        if (newTarget != null)
        {
            target = newTarget;
        }
    }

    protected GameObject findClosestUnit()
    {
        GameObject foundTarget = null;

        // Sphere cast around the unit for a target
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, enemySearchRadius, enemySearchLayer);
        float minimumDistance = float.MaxValue;
        // Find the closest enemy
        for (int hitIndex = 0; hitIndex < hitColliders.Length; hitIndex++)
        {
            IUnit enemyUnit = (IUnit)hitColliders[hitIndex].gameObject.GetComponent(typeof(IUnit));
            if (enemyUnit != null)
            {
                GameObject enemyFound = enemyUnit.getGameObject();
                float enemyDistance = Vector3.Distance(enemyFound.transform.position, gameObject.transform.position);
                if (enemyDistance < minimumDistance)
                {
                    foundTarget = enemyFound;
                    minimumDistance = enemyDistance;
                }
            }
        }

        return foundTarget;
    }

    IEnumerator attack()
    {
        anim.SetBool("Attacking", true);
        // Let the animation play for half a second before dealing damage to the target
        yield return new WaitForSeconds(.5f);

        // Make sure our target still exists
        if (target == null)
        {
            attacking = false;
            anim.SetBool("Attacking", false);
            yield break;
        }

        // Check if our target is within the attack radius and isn't already dying
        Vector3 distance = target.transform.position - transform.position;
        distance.y = 0.0f;
        if (distance.magnitude < attackRadius && !isDying)
        {
            IUnit unitToAttack = (IUnit)target.GetComponent(typeof(IUnit));
            if (unitToAttack != null)
            {
                unitToAttack.dealDamage(attackDamage);
            }
        }

        // Wait another second before attacking again
        yield return new WaitForSeconds(1.0f);
        attacking = false;
        anim.SetBool("Attacking", false);
    }

    public void dealDamage(int damage)
    {
        health -= damage;

        if (health > 0)
        {
            anim.Play("wound");
            source.PlayOneShot(woundSound);
        }
        else
        {
            source.PlayOneShot(killSound);
            kill();
        }
    }

    public void applyForce(Vector3 force)
    {
        GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
        GetComponent<Rigidbody>().isKinematic = false;
        StartCoroutine(restartAgent());
    }

    public void applyExplosiveForce(float force, Vector3 position, float radius)
    {
        GetComponent<Rigidbody>().AddExplosionForce(force, position, radius, 20.0f, ForceMode.Impulse);
        GetComponent<Rigidbody>().isKinematic = false;
        StartCoroutine(restartAgent());
    }

    IEnumerator restartAgent()
    {
        while (GetComponent<Rigidbody>().velocity.magnitude > 0.1)
        {
            yield return new WaitForSeconds(.1f);
        }
        agent.enabled = true;
    }

    void kill()
    {
        isDying = true;
        if (agent.enabled)
        {
            agent.enabled = false;
        }
        BoxCollider collider = gameObject.GetComponent<BoxCollider>();
        collider.enabled = false;
        StopCoroutine(attack());
        anim.SetBool("Attacking", false);
        anim.SetBool("Dead", true);
        StartCoroutine(despawnBody());
    }

    IEnumerator despawnBody()
    {
        yield return new WaitForSeconds(bodyDespawnTime);
        Destroy(gameObject);
    }

    public void slowDown()
    {
        agent.speed = 1;
    }

    public bool isUnitDying()
    {
        return isDying;
    }

    public bool isUnitAlly()
    {
        return isAlly;
    }

    public GameObject getGameObject()
    {
        return gameObject;
    }
}
